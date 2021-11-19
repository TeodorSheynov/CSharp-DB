using System.Xml;

namespace CarDealer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using AutoMapper;

    using Data;
    using DTO.ExportDto;
    using DTO.ImportDto;
    using Dtos.Import;
    using Models;

    public class StartUp
    {
        private static readonly IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        }));

        public static void Main(string[] args)
        {
            CarDealerContext dbContext = new CarDealerContext();

            //ResetDb(dbContext);
            //string inputSuppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //string resultSuppliers = ImportSuppliers(dbContext, inputSuppliersXml);
            //Console.WriteLine(resultSuppliers);

            //string inputPartsXml = File.ReadAllText("../../../Datasets/parts.xml");
            //string resultParts = ImportParts(dbContext, inputPartsXml);
            //Console.WriteLine(resultParts);

            //string inputCarsXml = File.ReadAllText("../../../Datasets/cars.xml");
            //string resultCars = ImportCars(dbContext, inputCarsXml);
            //Console.WriteLine(resultCars);

            //string inputCustomersXml = File.ReadAllText("../../../Datasets/customers.xml");
            //string resultCustomers = ImportCustomers(dbContext, inputCustomersXml);
            //Console.WriteLine(resultCustomers);

            //string inputSalesXml = File.ReadAllText("../../../Datasets/sales.xml");
            //string resultSales= ImportSales(dbContext, inputSalesXml);
            //Console.WriteLine(resultSales);
            Console.WriteLine(GetTotalSalesByCustomer(dbContext));

        }

        //Problem 09
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");
            XmlSerializer xmlSerializer = new XmlSerializer(
                typeof(ImportSupplierDto[]), xmlRoot);

            //Why? Bcos XmlSerializer library is old and is old styled...
            //Allow us to read the inputXml string chunk by chunk
            using StringReader stringReader = new StringReader(inputXml);

            ImportSupplierDto[] dtos = (ImportSupplierDto[])
                xmlSerializer.Deserialize(stringReader);

            ICollection<Supplier> suppliers = mapper.Map<Supplier[]>(dtos);

            //If this pass
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));

            var partDtos = (ImportPartDto[])xmlSerializer
                .Deserialize(new StringReader(inputXml));
            ICollection<Part> parts = mapper.Map<Part[]>(partDtos)
                .Where(x => context.Suppliers.Any(z => z.Id == x.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer =
                GenerateXmlSerializer("Cars", typeof(ImportCarDto[]));
            using StringReader stringReader = new StringReader(inputXml);

            ImportCarDto[] carDtos = (ImportCarDto[])
                xmlSerializer.Deserialize(stringReader);

            ICollection<Car> cars = new HashSet<Car>();
            //ICollection<PartCar> partCars = new HashSet<PartCar>();

            foreach (ImportCarDto carDto in carDtos)
            {
                Car c = mapper.Map<Car>(carDto);

                ICollection<PartCar> currentCarParts = new HashSet<PartCar>();
                foreach (int partId in carDto.Parts.Select(p => p.Id).Distinct())
                {
                    Part part = context
                        .Parts
                        .Find(partId);

                    if (part == null)
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        CarId = c.Id,
                        PartId = part.Id,
                    };
                    currentCarParts.Add(partCar);
                }

                c.PartCars = currentCarParts;
                cars.Add(c);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));

            var customersDto = (ImportCustomerDto[])xmlSerializer
                .Deserialize(new StringReader(inputXml));

            var customers = mapper.Map<Customer[]>(customersDto);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        //Problem 13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));

            var salesDtos = (ImportSaleDto[])xmlSerializer
                .Deserialize(new StringReader(inputXml));

            //May be good to check if CustomerId exists too
            var sales = mapper.Map<Sale[]>(salesDtos)
                .Where(s => context.Cars.Any(c => c.Id == s.CarId))
                .ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        //Problem 14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlSerializer xmlSerializer =
                GenerateXmlSerializer("cars", typeof(ExportCarsWithDistanceDto[]));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            ExportCarsWithDistanceDto[] carsDtos = context
                .Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarsWithDistanceDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance.ToString()
                })
                .ToArray();

            xmlSerializer.Serialize(stringWriter, carsDtos, namespaces);

            return sb.ToString().TrimEnd();
        }
        //Problem 15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter sw = new StringWriter(sb);

            XmlSerializer serializer = GenerateXmlSerializer("cars", typeof(ExportCarsFromMaker[]));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var cars = context
                .Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new ExportCarsFromMaker
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance.ToString()
                }).ToArray();
            serializer.Serialize(sw, cars, namespaces);

            return sb.ToString().TrimEnd();
        }
        //Problem 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter sw = new StringWriter(sb);

            XmlSerializer serializer = GenerateXmlSerializer("suppliers", typeof(ExportLocalSuppliersDto[]));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            ExportLocalSuppliersDto[] localSuppliersDtos = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportLocalSuppliersDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count,
                })
                .ToArray();
            serializer.Serialize(sw, localSuppliersDtos, namespaces);

            return sb.ToString().TrimEnd();
        }
        //Problem 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter sw = new StringWriter(sb);

            XmlSerializer serializer = GenerateXmlSerializer("cars", typeof(ExportCarPartsDto[]));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);
            var carParts = context
                .Cars
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Select(c => new ExportCarPartsDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance.ToString(),
                    Parts = c.PartCars.Select(cp => new ExportPart
                    {
                        Name = cp.Part.Name,
                        Price = cp.Part.Price.ToString(),
                    })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                })
                .Take(5)
                .ToArray();
            serializer.Serialize(sw, carParts, namespaces);
            return sb.ToString().TrimEnd();
        }
        //Problem 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter sw = new StringWriter(sb);

            XmlSerializer serializer = GenerateXmlSerializer("customers", typeof(ExportSalesbyCustomerDto[]));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            ExportSalesbyCustomerDto[] salesbyCustomerDtos = context
                .Sales
                .Select(s => new ExportSalesbyCustomerDto
                {
                    BoughtCars = s.Customer.Sales.Count.ToString(),
                    FullName = s.Customer.Name,
                    MoneySpent = s.Car.PartCars.Sum(x => x.Part.Price)

                })
                .OrderByDescending(c=>c.MoneySpent)
                .ToArray();

            serializer.Serialize(sw,salesbyCustomerDtos,namespaces);
            return sb.ToString().TrimEnd();
        }
        //Problem 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter sw = new StringWriter(sb);

            XmlSerializer xmlSerializer =
                GenerateXmlSerializer("sales", typeof(ExportSalesWithDiscountDto[]));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            ExportSalesWithDiscountDto[] dtos = context
                .Sales
                .Select(s => new ExportSalesWithDiscountDto()
                {
                    Car = new ExportSalesCarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TravelledDistance.ToString()
                    },
                    Discount = s.Discount.ToString(CultureInfo.InvariantCulture),
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price).ToString(CultureInfo.InvariantCulture),
                    PriceWithDiscount = (s.Car.PartCars.Sum(pc => pc.Part.Price) -
                                         s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100).ToString(CultureInfo.InvariantCulture)
                })
                .ToArray();

            xmlSerializer.Serialize(sw, dtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        private static XmlSerializer GenerateXmlSerializer(string rootName, Type dtoType)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(
                dtoType, xmlRoot);

            return xmlSerializer;
        }

        private static void ResetDb(CarDealerContext dbContext)
        {
            //First call -> Db not exist
            //Second call -> Db exists, First Delete, Then ReCreate
            //N call
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            Console.WriteLine("Db reset was successful!");
        }
    }
}
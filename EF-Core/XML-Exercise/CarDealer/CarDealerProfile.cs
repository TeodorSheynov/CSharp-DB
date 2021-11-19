using CarDealer.Models;
using CarDealer.Dtos.Import;

using AutoMapper;
using CarDealer.DTO.ExportDto;

namespace CarDealer
{
    using DTO.ImportDto;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDto, Supplier>();
            this.CreateMap<ImportPartDto, Part>();
            this.CreateMap<ImportCarDto, Car>();
            this.CreateMap<ImportCustomerDto, Customer>();
            this.CreateMap<ImportSaleDto, Sale>();
            this.CreateMap<Car, ExportCarsWithDistanceDto>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
           {
               cfg.AddProfile<ProductShopProfile>();
           }));
        public static void Main(string[] args)
        {

            ProductShopContext context = new ProductShopContext();
            //ResetDatabase(context);
            //var userInputJson = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, userInputJson));
            //var productInputJson = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productInputJson));
            //var categoryInputJson = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoryInputJson));
            //var categoryProductsInputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, categoryProductsInputJson));
            Console.WriteLine(GetUsersWithProducts(context));

        }
        //TODO: Query 1. Import Data

        //Query 2. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var usersDto = JsonConvert.DeserializeObject<ImportDtos.UserDto[]>(inputJson);
            var users = mapper.Map<User[]>(usersDto);
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }
        //Query 3. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var productsDto = JsonConvert.DeserializeObject<ImportDtos.ProductDto[]>(inputJson);
            var products = mapper.Map<Product[]>(productsDto);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }
        //Query 4. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categoriesDto = JsonConvert.DeserializeObject<ImportDtos.CategoryDto[]>(inputJson);
            var categories = mapper.Map<Category[]>(categoriesDto);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }
        //Query 5. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProductsDto = JsonConvert.DeserializeObject<ImportDtos.CategoryProductsDto[]>(inputJson);
            var categoryProducts = mapper.Map<CategoryProduct[]>(categoryProductsDto);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }
        //TODO:1. Query and Export Data
        //Query 6. Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .ProjectTo<ExportDtos.ProductDto>(mapper.ConfigurationProvider)
                .OrderBy(p=>p.Price)
                .ToArray();

            return SerilizeJson(products);

        }
        //Query 7. 
        public static string GetSoldProducts(ProductShopContext context)
        {

            var usersWithSoldItem = context
                .Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer.FirstName,
                            buyerLastName = p.Buyer.LastName
                        })

                })
                .OrderBy(u => u.lastName)
                .ThenBy(u => u.firstName)
                .ToArray();

           
            return SerilizeJson(usersWithSoldItem);
        }
        //Query 8. Export Categories by Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Where(c=>c.Name!= null)
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count(),
                    averagePrice = $"{c.CategoryProducts.Average(p => p.Product.Price):f2}",
                    totalRevenue = $"{c.CategoryProducts.Sum(p => p.Product.Price):f2}"
                })
                .OrderByDescending(c => c.productsCount)
                .ProjectTo<ExportDtos.CategoryDto>(mapper.ConfigurationProvider)
                .ToArray();

            
            return SerilizeJson(categories);
        }
        //Query 9. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersAndProducts = context
                .Users
                .Where(u => u.ProductsSold.Any())
                .Select(u => new
                {
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(b => b.Buyer != null),
                        products = u.ProductsSold
                            .Where(b=>b.Buyer!=null)
                            .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        }).ToList()
                    },
                })
                .OrderByDescending(u=>u.soldProducts.count)
                .ToList();

            return JsonConvert.SerializeObject(usersAndProducts, Formatting.Indented);


        }
        public static string SerilizeJson(Array entityCollection)
        {
            DefaultContractResolver resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = resolver,
            };

            var json = JsonConvert.SerializeObject(entityCollection,settings);

            return json;
        }
        public static void ResetDatabase(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            Console.WriteLine("Created successfully");
        }
    }
}
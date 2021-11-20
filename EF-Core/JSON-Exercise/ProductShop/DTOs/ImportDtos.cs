namespace ProductShop.DTOs
{
    public class ImportDtos
    {
        public class UserDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int? Age { get; set; }
        }
        public class ProductDto
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int SellerId { get; set; }
            public int? BuyerId { get; set; }
        }
        public class CategoryDto
        {
            public string Name { get; set; }
        }
        public class CategoryProductsDto
        {
            public int CategoryId { get; set; }
            public int ProductId { get; set; }
        }
    }
}
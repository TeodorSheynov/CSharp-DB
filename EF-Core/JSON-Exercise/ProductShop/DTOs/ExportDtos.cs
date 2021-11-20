namespace ProductShop.DTOs
{
    public class ExportDtos
    {
        public class ProductDto
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Seller { get; set; }
        }
        
        public class CategoryDto
        {
            public string Category { get; set; }
            public int ProductsCount { get; set; }
            public string AveragePrice { get; set; }
            public string TotalRevenue { get; set; }
        }
    }
}
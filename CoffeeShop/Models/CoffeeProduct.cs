namespace CoffeeShop.Models
{
    public class CoffeeProduct
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string RoastLevel { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
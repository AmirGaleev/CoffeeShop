namespace CoffeeShop.Models
{
    public class StoreSettings
    {
        public string StoreName { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public int PageSize { get; set; } = 12;
    }
}
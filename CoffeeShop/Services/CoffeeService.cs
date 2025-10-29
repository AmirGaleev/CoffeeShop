using CoffeeShop.Models;

namespace CoffeeShop.Services
{
    public class CoffeeService : ICoffeeService
    {
        private readonly List<CoffeeProduct> _products = new()
        {
            new CoffeeProduct 
            { 
                Id = 1, 
                Name = "Эфиопский Иргачеф", 
                Description = "Ароматный кофе с цветочными нотами", 
                Price = 1200, 
                Category = "Зерновой", 
                Origin = "Эфиопия",
                RoastLevel = "Средняя",
                Stock = 25,
                ImageUrl = "/images/ethiopian.jpg"
            },
            new CoffeeProduct 
            { 
                Id = 2, 
                Name = "Колумбийский Супремо", 
                Description = "Сбалансированный кофе с ореховыми нотами", 
                Price = 950, 
                Category = "Зерновой", 
                Origin = "Колумбия",
                RoastLevel = "Средняя",
                Stock = 30,
                ImageUrl = "/images/colombian.jpg"
            },
            new CoffeeProduct 
            { 
                Id = 3, 
                Name = "Бразильский Сантос", 
                Description = "Мягкий кофе с шоколадным послевкусием", 
                Price = 850, 
                Category = "Молотый", 
                Origin = "Бразилия",
                RoastLevel = "Темная",
                Stock = 20,
                ImageUrl = "/images/brazilian.jpg"
            },
            new CoffeeProduct 
            { 
                Id = 4, 
                Name = "Кения АА", 
                Description = "Яркий кофе с ягодными акцентами", 
                Price = 1350, 
                Category = "Зерновой", 
                Origin = "Кения",
                RoastLevel = "Светлая",
                Stock = 15,
                ImageUrl = "/images/kenya.jpg"
            }
        };

        public IEnumerable<CoffeeProduct> GetAllProducts() => _products;

        public CoffeeProduct? GetProductById(int id) => 
            _products.FirstOrDefault(p => p.Id == id);

        public IEnumerable<CoffeeProduct> GetProductsByCategory(string category) =>
            _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<string> GetCategories() =>
            _products.Select(p => p.Category).Distinct();
    }
}   
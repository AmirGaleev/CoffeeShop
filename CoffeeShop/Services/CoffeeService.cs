using CoffeeShop.Models;

namespace CoffeeShop.Services
{
    public class CoffeeService : ICoffeeService
    {
        private readonly IDatabaseService _databaseService;

        public CoffeeService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<CoffeeProduct> GetAllProducts() => _databaseService.GetAllProducts();

        public CoffeeProduct? GetProductById(int id) => _databaseService.GetProductById(id);

        public IEnumerable<CoffeeProduct> GetProductsByCategory(string category) =>
            _databaseService.GetProductsByCategory(category);

        public IEnumerable<string> GetCategories() => _databaseService.GetCategories();
    }
}
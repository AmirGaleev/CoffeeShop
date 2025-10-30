using CoffeeShop.Models;

namespace CoffeeShop.Services
{
    public interface IDatabaseService
    {
        IEnumerable<CoffeeProduct> GetAllProducts();
        CoffeeProduct? GetProductById(int id);
        IEnumerable<CoffeeProduct> GetProductsByCategory(string category);
        IEnumerable<string> GetCategories();
    }
}
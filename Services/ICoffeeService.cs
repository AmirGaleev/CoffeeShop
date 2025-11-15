using CoffeeShop.Models;

namespace CoffeeShop.Services
{
    public interface ICoffeeService
    {
        IEnumerable<CoffeeProduct> GetAllProducts();
        CoffeeProduct? GetProductById(int id);
        IEnumerable<CoffeeProduct> GetProductsByCategory(string category);
        IEnumerable<string> GetCategories();
    }
}
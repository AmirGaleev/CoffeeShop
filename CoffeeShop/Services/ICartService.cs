using CoffeeShop.Models;

namespace CoffeeShop.Services
{
    public interface ICartService
    {
        void AddToCart(CoffeeProduct product, int quantity = 1);
        void RemoveFromCart(int productId);
        void ClearCart();
        Dictionary<int, CartItem> GetCartItems();
        int GetCartItemsCount();
        decimal GetCartTotal();
    }
}
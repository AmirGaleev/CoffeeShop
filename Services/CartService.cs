using CoffeeShop.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CoffeeShop.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartService> _logger;
        private const string CartSessionKey = "ShoppingCart";

        public CartService(IHttpContextAccessor httpContextAccessor, ILogger<CartService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        private ISession Session 
        { 
            get 
            {
                var session = _httpContextAccessor.HttpContext?.Session;
                if (session == null)
                {
                    _logger.LogError("Сессия не доступна!");
                    throw new InvalidOperationException("Session is not available");
                }
                return session;
            }
        }

        public void AddToCart(CoffeeProduct product, int quantity = 1)
        {
            try
            {
                _logger.LogInformation("Начало добавления в корзину: ProductId={ProductId}, Name={ProductName}", 
                    product.Id, product.Name);

                var cart = GetCartItems();
                _logger.LogInformation("Текущая корзина: {Count} items", cart.Count);

                if (cart.ContainsKey(product.Id))
                {
                    cart[product.Id].Quantity += quantity;
                    _logger.LogInformation("Увеличено количество товара {ProductId} до {Quantity}", 
                        product.Id, cart[product.Id].Quantity);
                }
                else
                {
                    cart[product.Id] = new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = quantity,
                        ImageUrl = product.ImageUrl
                    };
                    _logger.LogInformation("Добавлен новый товар в корзину: {ProductName}", product.Name);
                }
                
                SaveCart(cart);
                _logger.LogInformation("Корзина успешно сохранена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в AddToCart для товара {ProductId}", product.Id);
                throw;
            }
        }

        public void RemoveFromCart(int productId)
        {
            try
            {
                _logger.LogInformation("Удаление товара из корзины: ProductId={ProductId}", productId);
                var cart = GetCartItems();
                cart.Remove(productId);
                SaveCart(cart);
                _logger.LogInformation("Товар удален из корзины");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении товара {ProductId} из корзины", productId);
                throw;
            }
        }

        public void ClearCart()
        {
            try
            {
                Session.Remove(CartSessionKey);
                _logger.LogInformation("Корзина очищена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке корзины");
                throw;
            }
        }

        public Dictionary<int, CartItem> GetCartItems()
        {
            try
            {
                var cartJson = Session.GetString(CartSessionKey);
                if (string.IsNullOrEmpty(cartJson))
                {
                    _logger.LogInformation("Корзина пуста");
                    return new Dictionary<int, CartItem>();
                }

                var cart = JsonSerializer.Deserialize<Dictionary<int, CartItem>>(cartJson) 
                    ?? new Dictionary<int, CartItem>();
                
                _logger.LogInformation("Загружена корзина с {Count} товарами", cart.Count);
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке корзины");
                return new Dictionary<int, CartItem>();
            }
        }

        public int GetCartItemsCount()
        {
            var count = GetCartItems().Values.Sum(item => item.Quantity);
            _logger.LogDebug("Количество товаров в корзине: {Count}", count);
            return count;
        }

        public decimal GetCartTotal()
        {
            var total = GetCartItems().Values.Sum(item => item.Price * item.Quantity);
            _logger.LogDebug("Сумма корзины: {Total}", total);
            return total;
        }

        private void SaveCart(Dictionary<int, CartItem> cart)
        {
            try
            {
                var cartJson = JsonSerializer.Serialize(cart);
                Session.SetString(CartSessionKey, cartJson);
                _logger.LogDebug("Корзина сохранена в сессию");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении корзины");
                throw;
            }
        }
    }
}
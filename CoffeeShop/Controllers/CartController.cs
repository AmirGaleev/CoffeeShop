using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Services;
using CoffeeShop.Models;
using Microsoft.Extensions.Logging;

namespace CoffeeShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICoffeeService _coffeeService;
        private readonly ILogger<CartController> _logger;

        public CartController(
            ICartService cartService, 
            ICoffeeService coffeeService,
            ILogger<CartController> logger)
        {
            _cartService = cartService;
            _coffeeService = coffeeService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                _logger.LogInformation("Получен запрос на добавление в корзину: ProductId={ProductId}, Quantity={Quantity}", 
                    request.ProductId, request.Quantity);

                var product = _coffeeService.GetProductById(request.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Товар с ID {ProductId} не найден", request.ProductId);
                    return Json(new { success = false, message = "Товар не найден" });
                }

                _logger.LogInformation("Найден товар: {ProductName}", product.Name);

                _cartService.AddToCart(product, request.Quantity);
                var cartCount = _cartService.GetCartItemsCount();
                var cartTotal = _cartService.GetCartTotal();
                
                _logger.LogInformation("Товар успешно добавлен в корзину. Корзина: {Count} товаров, сумма: {Total}", 
                    cartCount, cartTotal);

                return Json(new { 
                    success = true, 
                    message = $"{product.Name} добавлен в корзину",
                    cartCount = cartCount,
                    cartTotal = cartTotal
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критическая ошибка при добавлении в корзину для товара {ProductId}", request.ProductId);
                return Json(new { 
                    success = false, 
                    message = "Произошла ошибка при добавлении товара в корзину" 
                });
            }
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            try
            {
                _logger.LogInformation("Удаление товара {ProductId} из корзины", request.ProductId);
                _cartService.RemoveFromCart(request.ProductId);
                var cartCount = _cartService.GetCartItemsCount();
                var cartTotal = _cartService.GetCartTotal();
                
                return Json(new { 
                    success = true,
                    cartCount = cartCount,
                    cartTotal = cartTotal
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении товара {ProductId} из корзины", request.ProductId);
                return Json(new { success = false, message = "Ошибка при удалении товара" });
            }
        }

        public IActionResult Index()
        {
            try
            {
                var cartItems = _cartService.GetCartItems().Values;
                ViewBag.CartTotal = _cartService.GetCartTotal();
                _logger.LogInformation("Отображение корзины с {Count} товарами", cartItems.Count);
                return View(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке страницы корзины");
                ViewBag.CartTotal = 0;
                return View(new List<CartItem>());
            }
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            try
            {
                _cartService.ClearCart();
                _logger.LogInformation("Корзина очищена");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при очистке корзины");
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult GetCartInfo()
        {
            try
            {
                var cartInfo = new
                {
                    Count = _cartService.GetCartItemsCount(),
                    Total = _cartService.GetCartTotal()
                };
                
                _logger.LogDebug("Запрос информации о корзине: {Count} товаров, {Total} руб.", 
                    cartInfo.Count, cartInfo.Total);
                    
                return Json(cartInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении информации о корзине");
                return Json(new { Count = 0, Total = 0 });
            }
        }
    }

    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class RemoveFromCartRequest
    {
        public int ProductId { get; set; }
    }
}
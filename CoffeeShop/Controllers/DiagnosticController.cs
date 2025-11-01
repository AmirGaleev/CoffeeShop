using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Services;
using Microsoft.Extensions.Logging;

namespace CoffeeShop.Controllers
{
    public class DiagnosticController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICoffeeService _coffeeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DiagnosticController> _logger;

        public DiagnosticController(
            ICartService cartService,
            ICoffeeService coffeeService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<DiagnosticController> logger)
        {
            _cartService = cartService;
            _coffeeService = coffeeService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var diagnostics = new
            {
                SessionId = _httpContextAccessor.HttpContext?.Session?.Id ?? "No session",
                SessionKeys = _httpContextAccessor.HttpContext?.Session?.Keys ?? new List<string>(),
                ProductsCount = _coffeeService.GetAllProducts().Count(),
                CartItemsCount = _cartService.GetCartItemsCount(),
                CartTotal = _cartService.GetCartTotal(),
                HasHttpContext = _httpContextAccessor.HttpContext != null,
                HasSession = _httpContextAccessor.HttpContext?.Session != null
            };

            _logger.LogInformation("Диагностика: {@Diagnostics}", diagnostics);

            return Json(diagnostics);
        }

        [HttpPost]
        public IActionResult TestAddToCart(int productId = 1)
        {
            try
            {
                var product = _coffeeService.GetProductById(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Тестовый товар не найден" });
                }

                _cartService.AddToCart(product, 1);
                var cartCount = _cartService.GetCartItemsCount();

                return Json(new { 
                    success = true, 
                    message = $"Тестовый товар {product.Name} добавлен в корзину",
                    cartCount = cartCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Тест добавления в корзину провалился");
                return Json(new { 
                    success = false, 
                    message = $"Тест провален: {ex.Message}",
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Services;
using CoffeeShop.Models;

namespace CoffeeShop.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICoffeeService _coffeeService;

        public CatalogController(ICoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
        }

        public IActionResult Index()
        {
            var products = _coffeeService.GetAllProducts();
            ViewBag.Categories = _coffeeService.GetCategories();
            return View(products);
        }

        // Пользовательский маршрут: /catalog/category/{categoryName}
        public IActionResult Category(string categoryName)
        {
            var products = _coffeeService.GetProductsByCategory(categoryName);
            if (!products.Any())
            {
                return NotFound();
            }
            
            ViewData["Category"] = categoryName;
            ViewBag.Categories = _coffeeService.GetCategories();
            return View("Index", products);
        }

        // Пользовательский маршрут: /coffee/{id}/{name?}
        public IActionResult Details(int id)
        {
            var product = _coffeeService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // API метод возвращающий JSON (Занятие 10)
        [Route("/catalog/api/all")]
        public IActionResult ApiAll()
        {
            var products = _coffeeService.GetAllProducts();
            return Json(products);
        }

        [Route("/catalog/api/categories")]
        public IActionResult ApiCategories()
        {
            var categories = _coffeeService.GetCategories();
            return Json(categories);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CoffeeShop.Models;

namespace CoffeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreSettings _storeSettings;

        public HomeController(IOptions<StoreSettings> storeSettings)
        {
            _storeSettings = storeSettings.Value;
        }

        public IActionResult Index()
        {
            ViewData["StoreName"] = _storeSettings.StoreName;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        // Пример возврата разных типов ответов (Занятие 10)
        public IActionResult StoreInfo()
        {
            var storeInfo = new
            {
                Name = _storeSettings.StoreName,
                Currency = _storeSettings.Currency,
                PageSize = _storeSettings.PageSize
            };
            return Json(storeInfo);
        }

        public IActionResult DownloadCatalog()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", "catalog.pdf");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            return PhysicalFile(filePath, "application/pdf", "CoffeeCatalog.pdf");
        }
    }
}
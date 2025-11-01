using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CoffeeShop.Models;
using CoffeeShop.Services;

namespace CoffeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreSettings _storeSettings;
        private readonly IVisitCounterService _visitCounter;

        public HomeController(
            IOptions<StoreSettings> storeSettings,
            IVisitCounterService visitCounter)
        {
            _storeSettings = storeSettings.Value;
            _visitCounter = visitCounter;
        }

        public IActionResult Index()
        {
            // Увеличиваем счетчик посещений
            _visitCounter.IncrementVisitCount();
            var visitCount = _visitCounter.GetVisitCount();

            ViewData["StoreName"] = _storeSettings.StoreName;
            ViewData["VisitCount"] = visitCount;

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
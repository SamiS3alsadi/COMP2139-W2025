using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Lap_1.Models;

namespace Lap_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() => View();

        public IActionResult About() => View();

        [HttpGet]
        public IActionResult Search(string query, string type)
        {
            // Default to Projects if none selected
            type = string.IsNullOrWhiteSpace(type) ? "Projects" : type;

            if (type.Equals("Tasks", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Search", "ProjectTask", new { area = "ProjectManagement", query });
            }

            return RedirectToAction("Search", "Project", new { area = "ProjectManagement", query });
        }

        [Route("Home/NotFound")]
        public new IActionResult NotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}
using System.Diagnostics;
using COMP2139_ICE.Data;
using Lap_1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lap_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string? query, string? type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return RedirectToAction("Index");

            string search = query?.Trim() ?? string.Empty;

            if (type == "Projects")
            {
                return RedirectToAction(
                    "Search",
                    "Projects",
                    new { area = "ProjectManagement", searchString = search }
                );
            }

            if (type == "Tasks")
            {
                return RedirectToAction(
                    "Search",
                    "ProjectTask",
                    new { area = "ProjectManagement", searchString = search }
                );
            }

            return RedirectToAction("Index");
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
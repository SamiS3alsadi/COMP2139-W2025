using Microsoft.AspNetCore.Mvc;

namespace Lap_1.Controllers
{
    public class Home1Controller : Controller
    {
        public IActionResult Index() => View();

        public IActionResult About() => View();
        [HttpGet]
        public IActionResult Search(string query, string type)
        {
            // default to projects if nothing selected
            type = string.IsNullOrWhiteSpace(type) ? "Projects" : type;

            if (type.Equals("Tasks", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Search", "ProjectTask", new { area = "ProjectManagement", query });
            }

            return RedirectToAction("Search", "Projects", new { area = "ProjectManagement", query });
        }

        [Route("Home1/NotFound")]
        public new IActionResult NotFound()
        {
            return View();
        }
    }
}
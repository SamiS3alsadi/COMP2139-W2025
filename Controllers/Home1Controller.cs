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

            if (type == "Tasks")
                return RedirectToAction("Search", "ProjectTask", new { query });

            return RedirectToAction("Search", "Projects", new { query });
        }

        [Route("Home1/NotFound")]
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
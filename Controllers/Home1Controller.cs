using Microsoft.AspNetCore.Mvc;

namespace Lap_1.Controllers
{
    public class Home1Controller : Controller
    {
        public IActionResult Index() => View();

        public IActionResult About() => View();
    }
}
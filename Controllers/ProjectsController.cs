
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Lap_1.Data;
using Lap_1.Models;

namespace Lap_1.Controllers
{
    public class ProjectsController : Controller
    {
        private static readonly List<Project> _projects = new();

        public IActionResult Index() => View(_projects);

        public IActionResult Create() => View(new Project());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if (!ModelState.IsValid) return View(project);
            project.Id = _projects.Count == 0 ? 1 : _projects.Max(p => p.Id) + 1;
            project.CreatedAt = DateTime.Now;
            _projects.Add(project);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null) return NotFound();
            return View(project);
        }
    }
}
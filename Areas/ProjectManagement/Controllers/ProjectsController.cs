using Lap_1.Areas.ProjectManagement.Models;
using Lap_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lap_1.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var projects = _context.Projects
                .Include(p => p.Tasks)
                .ToList();
            return View(projects);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                // Convert to UTC before saving
                project.StartDate = ToUtc(project.StartDate);
                project.EndDate = ToUtc(project.EndDate);

                _context.Projects.Add(project);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        private DateTime ToUtc(DateTime input)
        {
            if (input.Kind == DateTimeKind.Utc)
                return input;

            if (input.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(input, DateTimeKind.Utc); // assume local is already UTC

            return input.ToUniversalTime();
        }

        [HttpGet("Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
                return NotFound();

            return View(project);
        }

        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectId", "Name", "Description")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Projects.Update(project);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }

            return View(project);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }

        [HttpGet("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var project = _context.Projects.Find(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
                return RedirectToActionPermanent("Index");
            }

            return NotFound();
        }

        // Lab 6 - Project Search Functionality
        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var projectsQuery = _context.Projects.AsQueryable();

            bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

            if (searchPerformed)
            {
                searchString = searchString.ToLower();
                projectsQuery = projectsQuery.Where(p =>
                    p.Name.ToLower().Contains(searchString) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchString)));
            }

            var projects = await projectsQuery.ToListAsync();

            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;

            return View("Index", projects);
        }
    }
}
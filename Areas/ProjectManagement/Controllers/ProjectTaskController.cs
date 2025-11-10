using Lap_1.Areas.ProjectManagement.Models;
using Lap_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lap_1.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTaskController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(int projectId)
        {
            var tasks = _context.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .ToList();

            ViewBag.ProjectId = projectId;
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var task = _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpGet]
        public IActionResult Create(int projectId)
        {
            var project = _context.Projects.Find(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var task = new ProjectTask
            {
                ProjectId = projectId,
                Title = "",
                Description = ""
            };

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                _context.ProjectTasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.ProjectTasks.Update(task);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_context.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var task = _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int projectTaskId)
        {
            var task = _context.ProjectTasks.Find(projectTaskId);
            if (task != null)
            {
                _context.ProjectTasks.Remove(task);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            return NotFound();
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search(int? projectId, string searchString)
        {
            var taskQuery = _context.ProjectTasks.AsQueryable();

            bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

            if (projectId.HasValue)
            {
                taskQuery = taskQuery.Where(t => t.ProjectId == projectId.Value);
            }

            if (searchPerformed)
            {
                searchString = searchString.ToLower();

                taskQuery = taskQuery.Where(t =>
                    t.Title.ToLower().Contains(searchString) ||
                    (t.Description != null && t.Description.ToLower().Contains(searchString))
                );
            }
            var tasks = await taskQuery.ToListAsync();

            ViewBag.ProjectId = projectId;
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;

            return View("Index", tasks);
        }
    }
}
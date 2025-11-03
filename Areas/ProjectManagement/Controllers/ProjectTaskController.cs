using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lap_1.Data;
using Lap_1.Areas.ProjectManagement.Models;

namespace Lap_1.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectTaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProjectTaskController(ApplicationDbContext context) => _context = context;

        // GET: /ProjectManagement/ProjectTask/Index?projectId=15
        [HttpGet]
        public async Task<IActionResult> Index(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) return NotFound();

            ViewBag.Project = project;

            var tasks = await _context.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .AsNoTracking()
                .OrderBy(t => t.TaskId)
                .ToListAsync();

            return View(tasks);
        }

        // GET: /ProjectManagement/ProjectTask/Details/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _context.ProjectTasks
                .Include(t => t.Project) // include related project 
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TaskId == id);

            if (task == null) return NotFound();
            return View(task);
        }

        // GET: /ProjectManagement/ProjectTask/Search?query=foo
        [HttpGet]
        public async Task<IActionResult> Search(string? query)
        {
            bool searched = !string.IsNullOrWhiteSpace(query);

            var tasks = string.IsNullOrWhiteSpace(query)
                ? await _context.ProjectTasks
                    .OrderBy(t => t.TaskId)
                    .AsNoTracking()
                    .ToListAsync()
                : await _context.ProjectTasks
                    .Where(t =>
                        (t.Title != null && t.Title.Contains(query)) ||
                        (t.Description != null && t.Description.Contains(query))) //First,it checks if Title exits (is not null)//only then , it runs .Contains (query) safely
                    .OrderBy(t => t.TaskId)
                    .ToListAsync();

            ViewBag.Query = query;
            ViewBag.Searched = searched;

            return View("Index", tasks);
        }

        // GET: /ProjectManagement/ProjectTask/Create?projectId=15
        [HttpGet]
        public async Task<IActionResult> Create(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) return NotFound();

            ViewBag.Project = project;
            return View(new ProjectTask { ProjectId = projectId });
        }

        // POST: /ProjectManagement/ProjectTask/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int projectId, ProjectTask task)
        {
            if (task.ProjectId == 0)
                task.ProjectId = projectId;

            if (!ModelState.IsValid)
            {
                ViewBag.Project = await _context.Projects.FindAsync(task.ProjectId);
                return View(task);
            }

            _context.ProjectTasks.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
        }

        // GET: /ProjectManagement/ProjectTask/Edit/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        // POST: /ProjectManagement/ProjectTask/Edit/5
        [HttpPost("{id:int}")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,Title,Description,ProjectId")] ProjectTask task)
        {
            if (id != task.TaskId) return NotFound();
            if (!ModelState.IsValid) return View(task);

            try
            {
                _context.ProjectTasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.ProjectTasks.AnyAsync(t => t.TaskId == id);
                if (!exists) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
        }

        // GET: /ProjectManagement/ProjectTask/Delete/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.ProjectTasks
                    .Include(t => t.Project)
                    .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TaskId == id);

            if (task == null) return NotFound();
            return View(task);
        }

        // POST: /ProjectManagement/ProjectTask/Delete/5
        [HttpPost("{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task == null) return NotFound();

            var projectId = task.ProjectId;
            _context.ProjectTasks.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { projectId });
        }
    }
}
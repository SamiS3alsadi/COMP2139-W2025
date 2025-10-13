using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lap_1.Data;
using Lap_1.Models;
using System.Linq;

namespace Lap_1.Controllers

{
    [Route("tasks")]
    public class ProjectTaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProjectTaskController(ApplicationDbContext context) => _context = context;
        //get projecttassk?projectId=15
        [HttpGet("")]
        public async Task<IActionResult> Index(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) return NotFound();

            ViewBag.Project = project;
            var tasks = await _context.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .OrderBy(t => t.ProjectTaskId)
                .ToListAsync();

            return View(tasks);
        }
        //get detail/5
        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);
            if (task == null) return NotFound();
            return View(task);
        }
        // GET: /ProjectTask/Search
        [HttpGet("search")]
        public async Task<IActionResult> Search(string query)
        {
            bool searched = !string.IsNullOrWhiteSpace(query);

            var tasks = string.IsNullOrEmpty(query)
                ? await _context.ProjectTasks
                    .OrderBy(t => t.ProjectTaskId)
                    .ToListAsync()
                : await _context.ProjectTasks
                    .Where(t => t.Title.Contains(query) || t.Description.Contains(query))
                    .OrderBy(t => t.ProjectTaskId)
                    .ToListAsync();

            ViewBag.Query = query;
            ViewBag.Searched = searched;

            return View("Index", tasks);
        }
        //Get; /ProjecCreate?projectId=15
        [HttpGet("create")]
        public async Task<IActionResult> Create(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) return NotFound();
            
            ViewBag.Project = project; 

            return View(new ProjectTask { ProjectId = projectId });
        }
        //project /create 
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int projectId, ProjectTask task)
        {
            //if (!ModelState.IsValid) return View(task);
            if (task.ProjectId == 0)
                task.ProjectId = projectId;
            
            if (!ModelState.IsValid) 
                return View(task);

            _context.ProjectTasks.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
        }
        // get; edit3
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }
        // post; edit3
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskId,Title,Description,ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId) return NotFound();
            if (!ModelState.IsValid) return View(task);

            try
            {
                _context.ProjectTasks.Update(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.ProjectTasks.AnyAsync(t => t.ProjectTaskId == id);
                if (!exists) return NotFound();
                throw;
            }
        }
        //get; delete3
        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.ProjectTaskId == id);
            if (task == null) return NotFound();
            return View(task);
        }
        //post; delete 3
        [HttpPost("delete/{id:int}"), ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }    
            var projectId = task.ProjectId;
            _context.ProjectTasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { projectId });
        }




    }
}
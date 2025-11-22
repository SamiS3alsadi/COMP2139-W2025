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
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.Tasks)
                .ToListAsync();

            return View(projects);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.StartDate = ToUtc(project.StartDate);
                project.EndDate = ToUtc(project.EndDate);

                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        private DateTime ToUtc(DateTime input)
        {
            if (input.Kind == DateTimeKind.Utc)
                return input;

            if (input.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(input, DateTimeKind.Utc);

            return input.ToUniversalTime();
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
                return NotFound();

            return View(project);
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            return View(project);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Description")] Project project)
        {
            if (id != project.ProjectId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Projects.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProjectExistsAsync(project.ProjectId))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
                return NotFound();

            return View(project);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var query = _context.Projects.AsQueryable();
            bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

            if (searchPerformed)
            {
                searchString = searchString.ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchString) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchString)));
            }

            var projects = await query
                .Include(p => p.Tasks)
                .ToListAsync();

            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;

            return View("Index", projects);
        }

        private async Task<bool> ProjectExistsAsync(int id)
        {
            return await _context.Projects.AnyAsync(e => e.ProjectId == id);
        }
    }
}
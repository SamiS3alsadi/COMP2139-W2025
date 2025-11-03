using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lap_1.Data;
using Lap_1.Areas.ProjectManagement.Models;

namespace Lap_1.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("ProjectManagement/[controller]/[action]")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /ProjectManagement/Projects
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .OrderByDescending(p => p.Id)
                .ToListAsync();
            return View(projects);
        }

        // GET: /ProjectManagement/Projects/Create
        [HttpGet]
        public IActionResult Create() => View(new Project());

        // POST: /ProjectManagement/Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (!ModelState.IsValid)
                return View(project);

            project.CreatedAt = DateTime.UtcNow;
            project.StartDate = ToUtc(project.StartDate);
            project.EndDate = ToUtc(project.EndDate);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /ProjectManagement/Projects/Details/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                return NotFound();
            return View(project);
        }

        // GET: /ProjectManagement/Projects/Edit/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();
            return View(project);
        }

        // POST: /ProjectManagement/Projects/Edit/5
        [HttpPost("{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,Status")] Project project)
        {
            if (id != project.Id)
                return NotFound();

            project.StartDate = ToUtc(project.StartDate);
            project.EndDate = ToUtc(project.EndDate);

            if (!ModelState.IsValid)
                return View(project);

            try
            {
                var existing = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (existing == null)
                    return NotFound();

                project.CreatedAt = existing.CreatedAt;
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /ProjectManagement/Projects/Delete/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                return NotFound();
            return View(project);
        }

        // POST: /ProjectManagement/Projects/DeleteConfirmed/5
        [HttpPost("{id:int}")]
        [ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /ProjectManagement/Projects/Search
        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            bool searched = !string.IsNullOrWhiteSpace(query);

            var projects = string.IsNullOrEmpty(query)
                ? await _context.Projects.OrderByDescending(p => p.Id).ToListAsync()
                : await _context.Projects
                    .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                    .OrderByDescending(p => p.Id)
                    .ToListAsync();

            ViewBag.Query = query;
            ViewBag.Searched = searched;
            return View("Index", projects);
        }

        private static DateTime? ToUtc(DateTime? dt)
        {
            if (!dt.HasValue) return null;
            var v = dt.Value;
            if (v.Kind != DateTimeKind.Utc)
                v = DateTime.SpecifyKind(v, DateTimeKind.Utc);
            return v.ToUniversalTime();
        }

        private bool ProjectExists(int id) =>
            _context.Projects.Any(e => e.Id == id);
    }
}
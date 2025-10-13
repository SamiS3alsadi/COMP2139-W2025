using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lap_1.Data;
using Lap_1.Models;
using System.Linq;

namespace Lap_1.Controllers
{
    [Route("projects")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProjectsController(ApplicationDbContext context) => _context = context;
        
        [HttpGet("")]

        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .OrderByDescending(p => p.Id)
                .ToListAsync();
            return View(projects);
        }
        [HttpGet("create")]


        public IActionResult Create() => View(new Project());
        
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (!ModelState.IsValid) return View(project);

            project.CreatedAt = DateTime.UtcNow;
            project.StartDate = ToUtc(project.StartDate);
            project.EndDate = ToUtc(project.EndDate);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet("details/{id:int}")]

        public async Task<IActionResult> Details(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return NotFound();
            return View(project);
        }
        [HttpGet("edit/{id:int}")]

        public IActionResult Edit(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null) return NotFound();
            return View(project);
        }
        [HttpPost("edit/{id:int}")]

        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,Status")] Project project)
        {
            if (id != project.Id) return NotFound();
            project.StartDate = ToUtc(project.StartDate);
            project.EndDate = ToUtc(project.EndDate);

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = _context.Projects
                        .AsNoTracking()
                        .FirstOrDefault(p => p.Id == id);
                    if (existing == null) return NotFound();
                    project.CreatedAt = existing.CreatedAt;
                    _context.Projects.Update(project);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }
        [HttpGet("delete/{id:int}")]


        public IActionResult Delete(int id)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null) return NotFound();
            return View(project);
        }
        [HttpPost("delete/{id:int}")]
        [ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var project = _context.Projects.Find(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        // helper normalize data values to utc so postgres accept them 
        private static DateTime? ToUtc(DateTime? dt)
        {
            if (!dt.HasValue) return null;
            var v = dt.Value;
            if (v.Kind != DateTimeKind.Utc)
                v = DateTime.SpecifyKind(v, DateTimeKind.Utc);
            return v.ToUniversalTime();
        }
        [HttpGet("search")]
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

        private bool ProjectExists(int id) => _context.Projects.Any(e => e.Id == id);
    }
}
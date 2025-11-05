using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Lap_1.Data;

namespace Lap_1.Areas.ProjectManagement.Components.ProjectSummary
{
    public class ProjectSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ProjectSummaryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Get total number of projects
            var totalProjects = _context.Projects.Count();

            // Pass data to the view
            ViewData["ProjectCount"] = totalProjects;

            // Render the view
            return View("Default");
        }
    }
}
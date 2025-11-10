using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lap_1.Data;
using System.Threading.Tasks;

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
            var totalProjects = await _context.Projects.CountAsync();
            var totalTasks = await _context.ProjectTasks.CountAsync();

            ViewData["TotalProjects"] = totalProjects;
            ViewData["TotalTasks"] = totalTasks;

            return View();
        }
    }
}
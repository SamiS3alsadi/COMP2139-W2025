using Lap_1.Areas.ProjectManagement.Models;
using Lap_1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lap_1.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("ProjectManagement/[controller]/[action]")]
    public class ProjectCommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectCommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(int projectId)
        {
            var comments = await _context.ProjectComments
                .Where(c => c.ProjectId == projectId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new
                {
                    c.CommentId,
                    c.Content,
                    CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                })
                .ToListAsync();

            return Json(comments);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddComment(int projectId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return BadRequest("Content cannot be empty.");

            var comment = new ProjectComment
            {
                ProjectId = projectId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _context.ProjectComments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                comment.CommentId,
                comment.Content,
                CreatedAt = comment.CreatedAt.ToString("yyyy-MM-dd HH:mm")
            });
        }
    }
}
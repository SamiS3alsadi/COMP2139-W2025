using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lap_1.Areas.ProjectManagement.Models
{
    public class ProjectComment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
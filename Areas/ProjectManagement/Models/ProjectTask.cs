using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lap_1.Areas.ProjectManagement.Models
{
    public class ProjectTask
    {
        [Key]
        [Column("ProjectTaskId")]                
        [Display(Name = "Task ID")]
        public int TaskId { get; set; }          

        [Required, StringLength(100)]
        [Display(Name = "Task Title")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Task Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        [Display(Name = "Project ID")]
        public int ProjectId { get; set; }

        [Display(Name = "Parent Project")]
        public Project? Project { get; set; }
    }
}
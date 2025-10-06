using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lap_1.Models
{public class ProjectTask
    {
    [Key]
    public int ProjectTaskId { get; set; }
    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;
    [StringLength(500)]
    public string? Description { get; set; }
    [Required]
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Lap_1.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required, StringLength(60)]
        public string Name { get; set; } = "";

        [Required, StringLength(200)]
        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        
        public string? Status { get; set; } // new, in progress, completed
        
        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}
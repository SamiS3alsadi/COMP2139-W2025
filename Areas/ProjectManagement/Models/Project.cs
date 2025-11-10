using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lap_1.Areas.ProjectManagement.Models
{
    public class Project
    {
        [Display(Name = "Project Id")]
        public int ProjectId { get; set; }  

        [Required, StringLength(60)]
        [Display(Name = "Project Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }   

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}", ApplyFormatInEditMode = false)]
        public DateTime EndDate { get; set; }     

        [StringLength(20)]
        [Display(Name = "Status")]
        public string? Status { get; set; }

        
        public List<ProjectTask>? Tasks { get; set; } = new();
    }
}
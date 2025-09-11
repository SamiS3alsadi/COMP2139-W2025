using System;
using System.ComponentModel.DataAnnotations;

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
    }
}
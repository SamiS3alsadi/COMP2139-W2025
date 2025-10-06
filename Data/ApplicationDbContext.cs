using Microsoft.EntityFrameworkCore;
using Lap_1.Models;   

namespace Lap_1.Data   
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectTask> ProjectTasks { get; set; } = null!;
    }
}
using FitnessMVC201.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace FitnessMVC201.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}

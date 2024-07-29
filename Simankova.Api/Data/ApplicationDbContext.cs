using Microsoft.EntityFrameworkCore;
using Simankova.Domain.Entities;

namespace Simankova.Api.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder
            optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Simankova.Api;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}

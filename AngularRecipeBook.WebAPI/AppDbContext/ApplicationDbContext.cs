using AngularRecipeBook.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace AngularRecipeBook.WebAPI.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public ApplicationDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Recipe>().ToTable("Recipes");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredients");
        }
    }
}

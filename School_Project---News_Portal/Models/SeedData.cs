using Microsoft.EntityFrameworkCore;

namespace School_Project___News_Portal.Models
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {




            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Categori 1", IsActive = true },
                new Category() { Id = 2, Name = "Categori 1", IsActive = true },
                new Category() { Id = 3, Name = "Categori 2", IsActive = true }
            );



        }
    }
}

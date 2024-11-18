using Microsoft.EntityFrameworkCore;

namespace School_Project___News_Portal.Models
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Category 1", IsActive = true },
                new Category() { Id = 2, Name = "Category 1", IsActive = true },
                new Category() { Id = 3, Name = "Category 2", IsActive = true }
            );

            modelBuilder.Entity<NewsItem>().HasData(
                new NewsItem() { Id=1, Title="News 1", Description="News Desc 1", IsActive=true, PhotoUrl="//1", CategoryId = 1 },
                new NewsItem() { Id=2, Title="News 2", Description="News Desc 2", IsActive=true, PhotoUrl="//2", CategoryId = 2 },
                new NewsItem() { Id=3, Title="News 3", Description="News Desc 3", IsActive=true, PhotoUrl="//3", CategoryId = 3 }
            );

        }
    }
}

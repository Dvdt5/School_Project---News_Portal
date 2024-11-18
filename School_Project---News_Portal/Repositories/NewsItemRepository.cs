using School_Project___News_Portal.Models;

namespace School_Project___News_Portal.Repositories
{
    public class NewsItemRepository : GenericRepository<NewsItem>
    {
        public NewsItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}

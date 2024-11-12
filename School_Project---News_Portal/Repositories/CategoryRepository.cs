using School_Project___News_Portal.Models;

namespace School_Project___News_Portal.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}

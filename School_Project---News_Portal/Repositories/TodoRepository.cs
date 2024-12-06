using School_Project___News_Portal.Models;

namespace School_Project___News_Portal.Repositories
{
    public class TodoRepository : GenericRepository<Todo>
    {
        public TodoRepository(AppDbContext context) : base(context)
        {
        }
    }
}

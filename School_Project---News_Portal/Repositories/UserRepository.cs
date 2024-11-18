using School_Project___News_Portal.Models;

namespace School_Project___News_Portal.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(AppDbContext context) : base(context) 
        {
            
        }
    }
}

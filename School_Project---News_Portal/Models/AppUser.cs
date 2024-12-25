using Microsoft.AspNetCore.Identity;

namespace School_Project___News_Portal.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
    }
}

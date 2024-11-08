using Microsoft.AspNetCore.Mvc;

namespace School_Project___News_Portal.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

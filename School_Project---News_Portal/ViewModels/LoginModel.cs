using System.ComponentModel.DataAnnotations;

namespace School_Project___News_Portal.ViewModels
{
    public class LoginModel
    {
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "Please enter an Username!")]
        public string UserName { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a Password!")]
        public string Password { get; set; }


        [Display(Name = "Remember Me")]
        public bool KeepMe { get; set; }
    }
}

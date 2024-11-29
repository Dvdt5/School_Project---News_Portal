using System.ComponentModel.DataAnnotations;

namespace School_Project___News_Portal.ViewModels
{
    public class UserModel : BaseModel
    {
        [Display(Name = "FullName")]
        [Required(ErrorMessage = "Please enter a name and surname!")]
        public string FullName { get; set; }



        [Display(Name = "UserName")]
        [Required(ErrorMessage = "Please enter a UserName!")]
        public string UserName { get; set; }



        [Display(Name = "Email")]
        [Required(ErrorMessage = "Enter an email!")]
        public string Email { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter a Password")]
        public string Password { get; set; }


        [Display(Name = "Role")]
        [Required(ErrorMessage = "Enter a role!")]
        public string Role { get; set; }

        [Display(Name = "PhotoUrl")]
        public string PhotoUrl { get; set; }
    }
}

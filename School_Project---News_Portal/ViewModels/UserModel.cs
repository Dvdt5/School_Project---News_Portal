using System.ComponentModel.DataAnnotations;

namespace School_Project___News_Portal.ViewModels
{
    public class UserModel 
    {
        public string Id { get; set; }

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
        [MinLength(6, ErrorMessage = "Password needs to be atleast 6 character!")]
        [RegularExpression("/(?=.*\\d)(?=.*[A-Z])(?=.*[\\W_]).+$/g", ErrorMessage = "Password needs atleast 1 number, 1 uppercase letter, 1 specail character!")]
        public string Password { get; set; }


        [Display(Name = "Role")]
        [Required(ErrorMessage = "Enter a role!")]
        public string Role { get; set; }

        [Display(Name = "PhotoUrl")]
        public string PhotoUrl { get; set; }
    }
}

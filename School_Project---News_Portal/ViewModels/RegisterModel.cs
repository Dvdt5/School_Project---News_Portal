using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace School_Project___News_Portal.ViewModels
{
    public class RegisterModel
    {
        [Display(Name = "Name / Surname")]
        [Required(ErrorMessage = "Please enter Fullname!")]
        public string FullName { get; set; }
        

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter an Username!")]
        public string UserName { get; set; }


        [Display(Name = "Mail")]
        [Required(ErrorMessage = "Enter a Mail!")]
        public string Email { get; set; }


        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter a Password!")]
        [MinLength(6, ErrorMessage = "Password needs to be atleast 6 character!")]
        [RegularExpression("/(?=.*\\d)(?=.*[A-Z])(?=.*[\\W_]).+$/g", ErrorMessage = "Password needs atleast 1 number, 1 uppercase letter, 1 specail character!")]
        public string Password { get; set; }


        [Display(Name = "Password confirm")]
        [Required(ErrorMessage = "Confirm Password!")]
        [Compare("Password", ErrorMessage = "Password doesn't match!")]
        public string PasswordConfirm { get; set; }


        [Display(Name = "Photo")]
        public IFormFile ?PhotoFile { get; set; }
    }
}

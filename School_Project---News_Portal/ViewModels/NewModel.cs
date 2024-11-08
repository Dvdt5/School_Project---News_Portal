using System.ComponentModel.DataAnnotations;

namespace School_Project___News_Portal.ViewModels
{
    public class NewModel : BaseModel
    {
        [Display(Name = "New's Title")]
        [Required(ErrorMessage = "Please enter a title!")]
        public string Title { get; set; }


        [Display(Name = "New's Description")]
        [Required(ErrorMessage = "Please enter a description!")]
        public string Description { get; set; }


        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please enter a category!")]
        public int CategoryId { get; set; }

    }
}

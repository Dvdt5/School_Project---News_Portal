using System.ComponentModel.DataAnnotations;

namespace School_Project___News_Portal.ViewModels
{
    public class NewsItemModel : BaseModel
    {
        [Display(Name = "News Item Title")]
        [Required(ErrorMessage = "Please enter a Title!")]
        public string Title { get; set; }


        [Display(Name = "News Item Description")]
        [Required(ErrorMessage = "Please enter a description!")]
        public string Description { get; set; }


        [Display(Name = "Photo")]
        public IFormFile ?PhotoFile { get; set; }

        public string ?PhotoUrl { get; set; }


        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please enter a category!")]
        public int CategoryId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace School_Project___News_Portal.ViewModels
{
    public class CategoryModel : BaseModel
    {
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        public string Name { get; set; }
    }
}

namespace School_Project___News_Portal.Models
{
    public class New : BaseEntity
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

namespace School_Project___News_Portal.Models
{
    public class NewsItem : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }
        
        public string PhotoUrl { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}

namespace School_Project___News_Portal.Models
{
    public class Todo : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int IsOK { get; set; }
    }
}

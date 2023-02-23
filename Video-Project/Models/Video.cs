namespace Video_Project.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Url { get; set; }
        public DateTime DatePosted { get; set; }
        public Boolean IsCurrent { get; set; }

    }
}

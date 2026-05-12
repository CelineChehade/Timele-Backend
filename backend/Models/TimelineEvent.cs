namespace Timele.Models
{
    public class TimelineEvent
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int Year { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Difficulty { get; set; } = string.Empty;
    }
}
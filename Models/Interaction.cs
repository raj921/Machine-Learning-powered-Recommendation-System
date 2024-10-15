namespace RecommendationSystem.Models
{
    public class Interaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public float Rating { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
namespace BookReviewsAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public decimal Rating { get; set; }
        public string Content { get; set; }
    }
}

using BookReviews.Domain.Models.DataModels;

namespace BookReviews.Domain.Models.DTOs.ExposedDTOs
{
    public class ReviewDTO
    {
        public BookDTO Book { get; set; }
        public UserDTO User { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Rating { get; set; }
        public string Content { get; set; }

        public ReviewDTO(Review review, BookDTO book)
        {
            Book = book;
            User = new UserDTO(review.User, mapLikedBooks: false);
            CreatedAt = review.CreatedAt;
            Rating = review.Rating;
            Content = review.Content;
        }
    }
}

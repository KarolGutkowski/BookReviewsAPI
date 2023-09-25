namespace BookReviewsAPI.Models
{
    public class Book
    {
        public int Id {get; set;}
        public string Title { get; set; }
        public int Year { get; set; }
        public List<Author> Authors { get; set; }

        public List<Review> Reviews { get; set; }

        public string Img { get; set; }
    }

}

using Dapper;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReviewsAPI.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Book> Books { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

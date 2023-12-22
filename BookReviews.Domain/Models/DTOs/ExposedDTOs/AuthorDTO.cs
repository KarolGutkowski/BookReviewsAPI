using BookReviews.Domain.Models.DataModels;

namespace BookReviews.Domain.Models.DTOs.ExposedDTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<BookDTO> Books { get; set; } = new List<BookDTO>();
        public DateTime DateOfBirth { get; set; }

        public AuthorDTO(Author author, bool mapBooks = false)
        {
            Id = author.Id;
            FirstName = author.FirstName;
            LastName = author.LastName;
            if(mapBooks)
            {
                foreach(var book in author.Books)
                {
                    Books.Add(new BookDTO(book,mapAuthors: false, mapReviews: false, mappedLikedByUsers: false));
                }
            }
            DateOfBirth = author.DateOfBirth;
        }
    }
}

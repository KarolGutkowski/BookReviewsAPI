using BookReviews.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Domain.Models.DTOs.ExposedDTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
        public ICollection<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();
        public string? Img { get; set; }
        public BookDTO(Book book, bool mapAuthors, bool mapReviews, bool mappedLikedByUsers)
        {
            Id = book.Id;
            Title = book.Title;
            Year = book.Year;
            Description = book.Description;
            if(mapAuthors && book.Authors is not null)
            {
                foreach(var author in book.Authors)
                {
                    // not mapping books in this case cause we dont need authors book details for other books page
                    // this can also cause a circular dependency
                    // where author requires list of books and book requires list of authors...
                    Authors.Add(new AuthorDTO(author, mapBooks: false));
                }
            }

            if(mapReviews)
            {
                foreach(var review in book.Reviews)
                {
                    Reviews.Add(new ReviewDTO(review, this));
                }
            }

            Img = book.Img;
        }
    }
}

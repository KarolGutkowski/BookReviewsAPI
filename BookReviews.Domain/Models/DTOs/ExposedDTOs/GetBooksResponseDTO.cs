using BookReviews.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Domain.Models.DTOs.ExposedDTOs
{
    public class GetBooksResponseDTO
    {
        public ICollection<Book> books { get; set; } = new List<Book>();
        public int totalPagesCount { get; set; }
    }
}

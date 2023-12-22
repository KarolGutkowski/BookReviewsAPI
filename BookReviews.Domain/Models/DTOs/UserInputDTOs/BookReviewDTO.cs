using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Domain.Models.DTOs
{
    public class BookReviewDTO
    {
        public int BookId { get; set; }
        public DateTime Date { get; set; }
        public decimal Rating { get; set; }
        public string Content { get; set; }
    }
}

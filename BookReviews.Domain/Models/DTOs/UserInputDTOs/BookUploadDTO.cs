using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReviews.Domain.Models.DTOs.UserInputDTOs
{
    public class BookUploadDTO
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string AuthorFirstName {get; set;}
        public string AuthorLastName { get; set; }
    }
}

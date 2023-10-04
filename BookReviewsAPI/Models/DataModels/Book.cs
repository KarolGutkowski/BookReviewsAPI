using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookReviewsAPI.Models
{
    public class Book
    {
        public int Id {get; set;}
        [Required]
        public string Title { get; set; }
        public int Year { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Author>? Authors { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Review>? Reviews { get; set; }
        public string? Img { get; set; }
    }

}

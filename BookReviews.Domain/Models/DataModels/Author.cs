using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookReviews.Domain.Models.DataModels;

public class Author
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }

    [JsonIgnore]
    public ICollection<Book> Books { get; set; }
    public DateTime DateOfBirth { get; set; }
}

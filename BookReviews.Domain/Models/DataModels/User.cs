using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookReviews.Domain.Models.DataModels;

public class User
{
    [JsonIgnore]
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<Book>? LikedBooks { get; set; }

    public string? ProfileImage { get; set; }
}

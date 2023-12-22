using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookReviews.Domain.Models.DataModels;

public class User
{
    [JsonIgnore]
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<Book>? LikedBooks { get; set; }
    public string? ProfileImage { get; set; }
    public bool IsAdmin { get; set; } = false;
}

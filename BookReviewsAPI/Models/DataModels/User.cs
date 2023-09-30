using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookReviewsAPI.Models
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}

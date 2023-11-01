using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookReviews.Domain.Models.DataModels;

public class Review
{
    public int Id { get; set; }
    [Required]
    public Book Book { get; set; }
    [Required]
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }

    [Precision(6,5)]
    public decimal Rating { get; set; }
    public string Content { get; set; }
}

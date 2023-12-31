﻿using BookReviews.Domain.Models.DTOs.UserInputDTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookReviews.Domain.Models.DataModels;

public class Book
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public int Year { get; set; }
    public string Description { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<Author>? Authors { get; set; } 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public string? Img { get; set; }
    [JsonIgnore]
    public ICollection<User>? LikedByUsers { get; set; }
    public static Book mapToBook(BookUploadDTO bookData, Author author)
    {
        var book = new Book();
        book.Title = bookData.Title;
        book.Year = bookData.Year;
        book.Description = bookData.Description;
        book.Authors = new List<Author>() { author };
        return book;
    }
}

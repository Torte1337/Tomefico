using System;
using System.ComponentModel.DataAnnotations;
using Tomefico.Enums;

namespace Tomefico.Models;

public class BookModel
{
    [Key] public Guid Id { get; set; }
    [Required] public string Title { get; set; }
    public string? Description { get; set; }
    public byte[]? CoverImage { get; set; }
    public AuthorModel Author { get; set; }
    [Required] public BookStatus Status { get; set; } = BookStatus.ToRead;
    public DateTime? StartedReadingAt { get; set; }
    public DateTime? FinischedReadingAt { get; set; }
    public float? PersonalRating { get; set; }
    public string? Notes { get; set; }
    public bool IsFavorite { get; set; }
}

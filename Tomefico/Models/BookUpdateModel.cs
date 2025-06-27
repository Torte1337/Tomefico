using System;
using Tomefico.Enums;

namespace Tomefico.Models;

public class BookUpdateModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? AuthorFirstname { get; set; }
    public string? AuthorLastname { get; set; }
    public string? Description { get; set; }
    public byte[]? CoverImage { get; set; }
    public BookStatus? Status { get; set; } = BookStatus.ToRead;
    public DateTime? StartedReadingAt { get; set; }
    public DateTime? FinischedReadingAt { get; set; }
    public int? PersonalRating { get; set; }
    public string? Notes { get; set; }
    public bool? IsFavorite { get; set; }
}

using System;
using Tomefico.Enums;

namespace Tomefico.Models;

public class LogModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public LogStatus LogStatus { get; set; }
    public DateTime CreatedAt { get; set;}
}

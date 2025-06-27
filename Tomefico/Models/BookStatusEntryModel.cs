using System;
using Tomefico.Enums;
using Tomefico.Extensions;

namespace Tomefico.Models;

public class BookStatusEntryModel
{
    public BookStatus Status { get; set; }
    public string DisplayName => Status.ToGerman();
}

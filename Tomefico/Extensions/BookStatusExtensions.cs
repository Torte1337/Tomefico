using System;
using CommunityToolkit.Maui.Core.Platform;
using Tomefico.Enums;

namespace Tomefico.Extensions;

public static class BookStatusExtensions
{
    public static string ToGerman(this BookStatus bookStatus)
    {
        return bookStatus switch
        {
            BookStatus.ToRead => "Noch zu lesen",
            BookStatus.Reading => "Wird gerade gelesen",
            BookStatus.Finished => "Fertig gelesen",
            BookStatus.Wishlist => "Wunschliste",
            _ => bookStatus.ToString()
        };
    }
}

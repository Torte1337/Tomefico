using System;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Tomefico.Models;

namespace Tomefico.Message;

public class MessageBookDetails : ValueChangedMessage<BookModel>
{
    public MessageBookDetails(BookModel book) : base(book)
    {
        
    }
}

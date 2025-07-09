using System;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Tomefico.Models;

namespace Tomefico.Message;

public class MessageEditBook : ValueChangedMessage<BookModel>
{
    public MessageEditBook(BookModel book) : base(book)
    {
        
    }
}

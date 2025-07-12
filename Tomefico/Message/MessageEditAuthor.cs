using System;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Tomefico.Models;

namespace Tomefico.Message;

public class MessageEditAuthor : ValueChangedMessage<AuthorModel>
{
    public MessageEditAuthor(AuthorModel author) : base(author)
    {
        
    }
}

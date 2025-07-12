using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tomefico.Extensions;
using Tomefico.Message;
using Tomefico.Models;

namespace Tomefico.ViewModels;

public partial class DetailBookViewModel : ObservableObject
{
    [ObservableProperty] private BookModel selectedBook = null;
    public Func<Task>? RequestClose { get; set; }


    public DetailBookViewModel()
    {
        WeakReferenceMessenger.Default.Unregister<MessageBookDetails>(this);
        WeakReferenceMessenger.Default.Register<MessageBookDetails>(this, (r, m) => OnSetBook(m));
    }
    public async void OnSetBook(MessageBookDetails msg)
    {
        if (msg == null)
            return;

        SelectedBook = msg.Value;
    }

    
    [RelayCommand]
    private async Task ClosePopup()
    {
        if (RequestClose is not null)
        {
            await RequestClose.Invoke();
            RequestClose = null;
        }
    }
}

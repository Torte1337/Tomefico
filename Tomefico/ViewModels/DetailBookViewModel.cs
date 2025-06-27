using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Extensions;
using Tomefico.Models;

namespace Tomefico.ViewModels;

public partial class DetailBookViewModel : ObservableObject
{
    [ObservableProperty] private BookModel selectedBook;
    public Func<Task>? RequestClose { get; set; }


    public DetailBookViewModel(BookModel book)
    {
        SelectedBook = book;
    }

    
    [RelayCommand]
    private async Task ClosePopup()
    {
        if (RequestClose is not null)
            await RequestClose.Invoke();
    }
}

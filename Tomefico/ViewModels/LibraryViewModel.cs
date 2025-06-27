using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tomefico.Message;
using Tomefico.Models;
using Tomefico.Service;
using Tomefico.Views.Popup;

namespace Tomefico.ViewModels;

public partial class LibraryViewModel : ObservableObject
{
    private readonly DataService dataService;
    private readonly IServiceProvider serviceProvider;
    [ObservableProperty] private ObservableCollection<BookModel> bookList = new ObservableCollection<BookModel>();

    public LibraryViewModel(DataService dataService, IServiceProvider serviceProvider)
    {
        this.dataService = dataService;
        this.serviceProvider = serviceProvider;
        WeakReferenceMessenger.Default.Register<MessageRefreshBookList>(this, async (r, m) => { await OnLoadBookList(); });
    }
    public async Task OnLoadBookList()
    {
        try
        {
            if (BookList != null && BookList.Count > 0)
                BookList.Clear();

            BookList = new ObservableCollection<BookModel>(await dataService.OnLoadAll());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    [RelayCommand]
    public async Task GoToCreateBook()
    {
        var popup = serviceProvider.GetRequiredService<CreateEditBookPopup>();
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }
    [RelayCommand]
    public async Task OnEdit(BookModel book)
    {
        //
    }
    [RelayCommand]
    public async Task OnDelete(BookModel book)
    {
        var confirmed = await Shell.Current.DisplayAlert("Löschen", $"Buch \"{book.Title}\" wirklich löschen?", "Ja", "Nein");
        if (confirmed)
        {
            var result = await dataService.OnDeleteBook(book.Id);
            if (result)
                BookList.Remove(book);
        }
    }
    [RelayCommand]
    public async Task OpenBookDetails(BookModel book)
    {
        try
        {
            var popup = serviceProvider.GetRequiredService<DetailBookPopup>();
            await Shell.Current.CurrentPage.ShowPopupAsync(popup);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR -> {ex.Message}");
        }

    }
}

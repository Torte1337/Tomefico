using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Models;
using Tomefico.Views.Popup;

namespace Tomefico.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IServiceProvider serviceProvider;
    [ObservableProperty] ObservableCollection<BookModel> finishedReadingList = new();
    [ObservableProperty] ObservableCollection<BookModel> readingList = new();
    [ObservableProperty] ObservableCollection<BookModel> todoreadingList = new();
    [ObservableProperty] ObservableCollection<BookModel> wishList = new();
    [ObservableProperty] ObservableCollection<BookModel> favoriteList = new();
    [ObservableProperty] ObservableCollection<AuthorModel> authorList = new();
    [ObservableProperty] ObservableCollection<BookModel> popupBookList = new();
    public Func<Task>? RequestClose { get; set; }
    public DashboardViewModel(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;

        for (int x = 0; x < 10; x++)
        {
            var book = new BookModel
            {
                Id = Guid.NewGuid(),
                Title = $"Testbuch " + x.ToString(),
                IsFavorite = false,
                Status = Enums.BookStatus.Reading
            };
            ReadingList.Add(book);
        }


        FavoriteList = new ObservableCollection<BookModel>();
        for (int i = 0; i < 5; i++)
        {
            var book = new BookModel
            {
                Id = Guid.NewGuid(),
                Title = $"Testbuch " + i.ToString(),
                IsFavorite = true,
                Status = Enums.BookStatus.Reading
            };
            FavoriteList.Add(book);
        }
        for (int p = 0; p < 5; p++)
        {
            var author = new AuthorModel
            {
                Id = Guid.NewGuid(),
                Firstname = "Testvorname",
                Surname = "Testnachname " + p.ToString()
            };
            AuthorList.Add(author);
        }

    }
    [RelayCommand]
    public async Task OpenWishlist()
    {
        OnClearPopuplist();
        PopupBookList = WishList;
        await OpenPopup();
    }
    [RelayCommand]
    public async Task OpenToReadList()
    {
        OnClearPopuplist();
        PopupBookList = TodoreadingList;
        await OpenPopup();
    }
    [RelayCommand]
    public async Task OpenReadingList()
    {
        OnClearPopuplist();
        PopupBookList = new ObservableCollection<BookModel>(ReadingList);
        await OpenPopup();
    }
    [RelayCommand]
    public async Task OpenFinishedList()
    {
        OnClearPopuplist();
        PopupBookList = FinishedReadingList;
        await OpenPopup();
    }
    private void OnClearPopuplist()
    {
        if (PopupBookList != null && PopupBookList.Count > 0)
            PopupBookList.Clear();
    }
    private async Task OpenPopup()
    {
        var popup = serviceProvider.GetRequiredService<BookListPopup>();
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }
    [RelayCommand]
    public async Task OnClosePopup()
    {
        if (RequestClose is not null)
            await RequestClose.Invoke();
    }
    [RelayCommand]
    public async Task OnOpenDetailPopup(BookModel selectedBook)
    {
        var detailPopup = new DetailBookPopup(selectedBook);
        await Shell.Current.CurrentPage.ShowPopupAsync(detailPopup);
    }


}
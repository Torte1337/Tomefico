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
    [ObservableProperty] private ObservableCollection<BookModel> filteredBookList = new ObservableCollection<BookModel>();
    [ObservableProperty] private ObservableCollection<AuthorModel> authorList = new ObservableCollection<AuthorModel>();
    [ObservableProperty] private AuthorModel selectedAuthor = null;

    public LibraryViewModel(DataService dataService, IServiceProvider serviceProvider)
    {
        this.dataService = dataService;
        this.serviceProvider = serviceProvider;

    }

    public async Task OnLoadLists()
    {
        try
        {
            BookList = new ObservableCollection<BookModel>(await dataService.OnLoadAll());

            var tempAuthorList = new List<AuthorModel>();

            foreach (var book in BookList)
                tempAuthorList.Add(book.Author);

            AuthorList = new ObservableCollection<AuthorModel>(tempAuthorList.GroupBy(x => new { x.Firstname, x.Surname}).Select(x => x.First()).OrderBy(x => x.Surname).ThenBy(x => x.Firstname).ToList());
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
}

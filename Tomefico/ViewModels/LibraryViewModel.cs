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
    private List<AuthorModel> allAuthors = new();
    private List<BookModel> allBooks = new();
    [ObservableProperty] private ObservableCollection<string> authorInitals = new();
    [ObservableProperty] private ObservableCollection<AuthorModel> filteredAuthors = new();
    [ObservableProperty] private string selectedInitial;

    public LibraryViewModel(DataService dataService, IServiceProvider serviceProvider)
    {
        this.dataService = dataService;
        this.serviceProvider = serviceProvider;
        _ = OnLoadLists();
        WeakReferenceMessenger.Default.Register<MessageRefreshBookList>(this, OnRefresh);
    }
    public async Task OnLoadLists()
    {
        try
        {
            allAuthors = await dataService.OnGetAuthorList();
            allBooks = await dataService.OnLoadAll();

            foreach (var author in allAuthors)
            {
                author.Books = allBooks
                    .Where(b => b.Author?.Id == author.Id)
                    .ToList();
            }

            // Nur Buchstaben, die vorkommen
            var letters = allAuthors
                .Select(a => a.Surname.Substring(0, 1).ToUpper())
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            AuthorInitals = new ObservableCollection<string>(letters);

            if (letters.Count > 0)
            {
                var firstLetter = letters[0];
                await OnSelectInitial(firstLetter);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    public async Task OnSelectInitial(string inital)
    {
        if (string.IsNullOrWhiteSpace(inital))
            return;
            
        SelectedInitial = inital;
        
        var filtered = allAuthors
            .Where(a => a.Surname.StartsWith(inital, StringComparison.OrdinalIgnoreCase))
            .ToList();

        FilteredAuthors = new ObservableCollection<AuthorModel>(filtered);
    }
    [RelayCommand]
    public async Task GoToCreateBook()
    {
        var popup = serviceProvider.GetRequiredService<CreateEditBookPopup>();
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }
    public async Task GoToEditBook(BookModel book)
    {
        var popup = serviceProvider.GetRequiredService<EditBookPopup>();
        WeakReferenceMessenger.Default.Send(new MessageEditBook(book));
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }
    public void OnRefresh(object recipient, MessageRefreshBookList msg)
    {
        _ = OnLoadLists();
    }

    [RelayCommand]
    public async Task OnClickedBook(BookModel book)
    {
        if (book == null)
            return;

        var result = await Shell.Current.DisplayActionSheet("Was möchtest du tun?", "Abbrechen", null, "Bearbeiten", "Löschen");
        if (result == null)
            return;

        if (result == "Abbrechen")
            return;

        if (result == "Bearbeiten")
        {
            await GoToEditBook(book);
            await OnLoadLists();
            return;
        }
        else if (result == "Löschen")
        {
            var deleteResult = await Shell.Current.DisplayActionSheet("Wirklich löschen", "Abbrechen", null, "Ja");
            if (deleteResult == "Ja")
            {
                var deleteResultDatabase = await dataService.OnDeleteBook(book.Id);
                if (deleteResultDatabase)
                    await Shell.Current.DisplayAlert("Erfolg", "Buch wurde gelöscht", "Ok");
                else
                    await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetreten", "Ok");


                await OnLoadLists();
                return;
            }
        }
    }

}

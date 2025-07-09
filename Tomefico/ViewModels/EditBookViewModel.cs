using System;
using System.Collections.ObjectModel;
using Android.Service.Autofill;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tomefico.Enums;
using Tomefico.Message;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.ViewModels;

public partial class EditBookViewModel : ObservableObject
{
    private readonly LogService logService;
    private readonly DataService dataService;
    private BookModel book;
    [ObservableProperty] private string bookTitle = "";
    [ObservableProperty] private string bookDescription = "";
    [ObservableProperty] private BookStatusEntryModel selectedStatusEntry = null;
    [ObservableProperty] private ObservableCollection<AuthorModel> authorList = new();
    [ObservableProperty] private AuthorModel? selectedAuthor = null;
    [ObservableProperty] private byte[]? bookCover;
    [ObservableProperty] private List<BookStatusEntryModel> statusDisplayList = Enum.GetValues(typeof(BookStatus)).Cast<BookStatus>().Select(s => new BookStatusEntryModel { Status = s }).ToList();
    public Func<Task>? RequestClose { get; set; }

    public EditBookViewModel(LogService logService, DataService dataService)
    {
        this.logService = logService;
        this.dataService = dataService;
    }
    public void OnRegisterMessage()
    {
        WeakReferenceMessenger.Default.Unregister<MessageEditBook>(this);
        WeakReferenceMessenger.Default.Register<MessageEditBook>(this, (r, m) => OnSetSelectedBook(m));
    }
    private async Task OnLoadList()
    {
        var list = await dataService.OnGetAuthorList();
        AuthorList = new ObservableCollection<AuthorModel>(list);
    }
    public async void OnSetSelectedBook(MessageEditBook msg)
    {
        if (msg.Value == null)
        return;

        book = msg.Value;
        BookTitle = msg.Value.Title;
        BookDescription = msg.Value.Description;
        BookCover = msg.Value.CoverImage;
        SelectedStatusEntry = StatusDisplayList.FirstOrDefault(x => x.Status == msg.Value.Status);

        // Autorenliste sauber laden
        var list = await dataService.OnGetAuthorList();
        AuthorList = new ObservableCollection<AuthorModel>(list);

        SelectedAuthor = AuthorList.FirstOrDefault(x => x.Id == msg.Value.Author.Id);
    }

    [RelayCommand]
    public async Task OnSaveBook()
    {
        book.Title = BookTitle;
        book.Description = BookDescription;
        book.Author = SelectedAuthor;
        book.CoverImage = BookCover;
        book.Status = SelectedStatusEntry.Status;

        var UpdatedBook = new BookUpdateModel
        {
            Id = book.Id,
            Author = SelectedAuthor,
            CoverImage = BookCover,
            Status = SelectedStatusEntry.Status,
            Title = BookTitle,
            Description = BookDescription
        };

        var result = await dataService.OnUpdateBook(UpdatedBook);
        if (result)
        {
            await Shell.Current.DisplayAlert("Erfolg", "Buch erfolgreich aktualisiert", "Ok");
            if (RequestClose is not null)
            {
                await RequestClose.Invoke();
                RequestClose = null;
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Fehler", "Buch konnte nicht aktualisiert werden", "Ok");
        }
    }
    [RelayCommand]
    public async Task OnPickCoverImage()
    {
        try
        {
            if (!MediaPicker.IsCaptureSupported)
                return;

            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Buchcover auswählen"
            });

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                BookCover = memoryStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Auswählen des Covers", ex.Message, DateTime.Now, LogStatus.Error);
            return;
        }
    }
    [RelayCommand]
    public async Task OnCancel()
    {
        BookTitle = "";
        BookDescription = "";
        SelectedAuthor = null;
        BookCover = null;
        SelectedStatusEntry = null;
        book = null;
        if (RequestClose is not null)
        {
            await RequestClose.Invoke();
            RequestClose = null;
        }
    }
    
}

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tomefico.Enums;
using Tomefico.Extensions;
using Tomefico.Message;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.ViewModels;

public partial class CreateBookViewModel : ObservableObject
{
    private readonly LogService logService;
    private readonly DataService dataService;
    [ObservableProperty] private string bookTitle = "";
    [ObservableProperty] private string bookDescription = "";
    [ObservableProperty] private string bookAuthor = "";
    [ObservableProperty] private string bookAuthorFirstname = "";
    [ObservableProperty] private string bookAuthorLastname = "";
    [ObservableProperty] private BookStatusEntryModel selectedStatusEntry;
    [ObservableProperty] private ObservableCollection<AuthorModel> authorList = new();
    [ObservableProperty] private AuthorModel? selectedAuthor = null;
    [ObservableProperty] private byte[]? bookCover;
    [ObservableProperty] private List<BookStatusEntryModel> statusDisplayList = Enum.GetValues(typeof(BookStatus)).Cast<BookStatus>().Select(s => new BookStatusEntryModel { Status = s }).ToList();
    public Func<Task>? RequestClose { get; set; }
    public CreateBookViewModel(LogService logService, DataService dataService)
    {
        this.logService = logService;
        this.dataService = dataService;
        SelectedStatusEntry = StatusDisplayList.First();
        _ = OnLoadAuthorList();
    }
    private async Task OnLoadAuthorList()
    {
        var authors = await dataService.OnGetAuthorList();
        AuthorList = new ObservableCollection<AuthorModel>(authors);
    }
    [RelayCommand]
    public async Task OnCreateBook()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(BookTitle))
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte gebe einen Buchtitel ein!", "Ok");
                return;
            }
            if (string.IsNullOrWhiteSpace(BookDescription))
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte gebe eine Buchbeschreibung ein!", "Ok");
                return;
            }

            if (SelectedAuthor == null)
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte wähle einen Autor aus!", "Ok");
                return;
            }

            var newBook = new BookModel
            {
                Id = Guid.NewGuid(),
                Title = BookTitle,
                Description = BookDescription,
                CoverImage = BookCover ?? null,
                Status = SelectedStatusEntry?.Status ?? BookStatus.ToRead,
                Author = SelectedAuthor
            };

            var result = await dataService.OnSaveNewBook(newBook);
            if (result)
            {
                await Shell.Current.DisplayAlert("Info", "Buch wurde erfolgreich erstellt", "Ok");
                

                WeakReferenceMessenger.Default.Send(new MessageRefreshBookList());


                if (RequestClose is not null)
                {
                    await RequestClose.Invoke();
                    RequestClose = null;
                }
            }
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Erstellen", ex.Message, DateTime.Now, LogStatus.Error);
            return;
        }
        finally
        {
            BookTitle = "";
            BookDescription = "";
            BookAuthor = "";
            SelectedStatusEntry = null;
            BookCover = null;
            SelectedAuthor = null;
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
        BookAuthor = "";
        SelectedStatusEntry = null;
        BookCover = null;
        SelectedAuthor = null;       
        if (RequestClose is not null)
        {
            await RequestClose.Invoke();
            RequestClose = null;
        }
    }
}

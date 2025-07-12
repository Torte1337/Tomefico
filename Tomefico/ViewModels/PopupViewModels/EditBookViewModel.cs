using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tomefico.Enums;
using Tomefico.Message;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.ViewModels.PopupViewModels;

public partial class EditBookViewModel : PopupViewModelBase
{
    public Func<Task>? RequestClose { get; set; }

    private BookModel selectedBook;

    [ObservableProperty] private string bookTitle = "";
    [ObservableProperty] private string bookDescription = "";
    [ObservableProperty] private byte[]? bookCover = null;

    [ObservableProperty] private List<BookStatusEntryModel> statusDisplayList;
    [ObservableProperty] private BookStatusEntryModel selectedStatusEntry;

    [ObservableProperty] private ObservableCollection<AuthorModel> authorList = new();
    [ObservableProperty] private AuthorModel? selectedAuthor = null;

    public EditBookViewModel(DataService dataService) : base(dataService)
    {
        InitializeStaticLists();
        ResetForm();
        _ = LoadAuthorListAsync();
        WeakReferenceMessenger.Default.Unregister<MessageEditBook>(this);
        WeakReferenceMessenger.Default.Register<MessageEditBook>(this,OnSetBook);
    }

    private async Task LoadAuthorListAsync()
    {
        try
        {
            var authors = await dataService.OnGetAuthorList();
            AuthorList = new ObservableCollection<AuthorModel>(authors);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", $"Autoren konnten nicht geladen werden: {ex.Message}", "Ok");
        }
    }
    
    private void InitializeStaticLists()
    {
        StatusDisplayList = Enum.GetValues(typeof(BookStatus))
            .Cast<BookStatus>()
            .Select(s => new BookStatusEntryModel { Status = s })
            .ToList();
    }
    private async void OnSetBook(object recipient, MessageEditBook msg)
    {
        if (msg == null || msg.Value == null)
        {
            if (RequestClose is not null)
            {
                await RequestClose.Invoke();
                RequestClose = null;
            }
        }
        selectedBook = msg.Value;
        BookTitle = msg.Value.Title;
        BookDescription = msg.Value.Description;
        SelectedAuthor = AuthorList.FirstOrDefault(x => x.Id == msg.Value.Author.Id);
        SelectedStatusEntry = StatusDisplayList.FirstOrDefault(x => x.Status == msg.Value.Status);
        BookCover = msg.Value.CoverImage;
    }
    private async Task OnUpdateBook()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(BookTitle))
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte gebe einen Titel für das Buch ein!", "Ok");
                return;
            }
            // if (string.IsNullOrWhiteSpace(BookDescription))
            // {
            //     await Shell.Current.DisplayAlert("Fehler", "Bitte gebe eine Beschreibung für das Buch ein!", "Ok");
            //     return;
            // }
            if (SelectedAuthor == null)
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte wähle einen Autor aus!", "Ok");
                return;
            }
            if (SelectedStatusEntry == null)
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte wähle einen Status für das Buch aus!", "Ok");
                return;
            }


            var updatedBook = new BookUpdateModel
            {
                Id = selectedBook.Id,
                Title = BookTitle,
                Description = BookDescription,
                Author = SelectedAuthor,
                CoverImage = BookCover ?? null,
                Status = SelectedStatusEntry.Status
            };

            var result = await dataService.OnUpdateBook(updatedBook);
            if (result)
            {
                WeakReferenceMessenger.Default.Send(new MessageRefreshBookList());
                await Shell.Current.DisplayAlert("Erfolgreich", $"Das Buch mit dem Titel {BookTitle} wurde aktualisiert!", "Ok");
                ResetForm();
                selectedBook = null;
                if (RequestClose is not null)
                {
                    await RequestClose.Invoke();
                    RequestClose = null;
                }
            }
            else
                await Shell.Current.DisplayAlert("Fehler", $"Das Buch mit dem Titel {BookTitle} konnte nicht aktualisiert werden!", "Ok");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", $"Typ: {ex.GetType().Name}\nNachricht: {ex.Message}", "Ok");
        }
    }
    private void ResetForm()
    {
        BookTitle = "";
        BookDescription = "";
        BookCover = null;
        SelectedAuthor = null;
        SelectedStatusEntry = StatusDisplayList.FirstOrDefault();
    }



    [RelayCommand]
    public async Task OnSave()
    {
        await OnUpdateBook();
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
            Console.WriteLine(ex.Message);
            return;
        }
    }
    [RelayCommand]
    public async Task OnCancel()
    {
        ResetForm();
        if (RequestClose is not null)
        {
            await RequestClose.Invoke();
            RequestClose = null;
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Enums;
using Tomefico.Models;
using Tomefico.Service;
using Tomefico.Views.Popup;

namespace Tomefico.ViewModels;

public partial class AuthorViewModel : ObservableObject
{
    [ObservableProperty] private string authorFirstname = "";
    [ObservableProperty] private string authorSurname = "";
    public Func<Task>? RequestClose { get; set; }
    [ObservableProperty] private ObservableCollection<AuthorModel> authorList = new();
    private readonly LogService logService;
    private readonly DataService dataService;
    private readonly IServiceProvider serviceProvider;
    public AuthorViewModel(LogService logService, DataService dataService, IServiceProvider serviceProvider)
    {
        this.logService = logService;
        this.dataService = dataService;
        this.serviceProvider = serviceProvider;
        _ = OnLoadAuthorList();
    }
    public async Task OnLoadAuthorList()
    {
        var authors = await dataService.OnGetAuthorList();
        if (authors == null)
        {
            await Shell.Current.DisplayAlert("Fehler", "Liste der Authoren konnte nicht geladen werden, bitte versuche es erneut.", "Ok");
            return;
        }
        if(AuthorList != null && AuthorList.Count > 0)
            AuthorList.Clear();

        AuthorList = new ObservableCollection<AuthorModel>(authors);
    }
    [RelayCommand]
    public async Task OnShowPopupCreateAuthor()
    {
        OnClearFields();
        try
        {
            var popup = serviceProvider.GetRequiredService<AuthorCreateEditPopup>();
            await Shell.Current.CurrentPage.ShowPopupAsync(popup);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DEBUG] ERROR {ex.Message}");
        }

    }
    [RelayCommand]
    public async Task OnShowPopupEditAuthor(AuthorModel author)
    {
        OnClearFields();
        if (author == null)
        {
            await Shell.Current.DisplayAlert("Fehler", "Daten des Authors sind Fehlerhaft und können nicht geladen werden.", "Ok");
            return;
        }
        AuthorFirstname = author.Firstname;
        AuthorSurname = author.Surname;
        var popup = serviceProvider.GetRequiredService<AuthorCreateEditPopup>();
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }
    [RelayCommand]
    public async Task OnSaveAuthor()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(AuthorFirstname))
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte gebe einen Vornamen ein.", "Ok");
                return;
            }
            if (string.IsNullOrWhiteSpace(AuthorSurname))
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte gebe einen Nachnamen ein.", "Ok");
                return;
            }

            var newAuthor = new AuthorModel
            {
                Id = Guid.NewGuid(),
                Firstname = AuthorFirstname,
                Surname = AuthorSurname,
                Name = AuthorFirstname + " " + AuthorSurname
            };

            await dataService.OnAddAuthor(newAuthor);
            await Shell.Current.DisplayAlert("Erfolg", "Author wurde erfolgreich hinzugefügt.", "Ok");
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Speichern des Authors", ex.Message, DateTime.Now, LogStatus.Error);
            return;
        }
        finally
        {
            if (RequestClose is not null)
            {
                await RequestClose.Invoke();
                RequestClose = null;
            }
            OnClearFields();
            await OnLoadAuthorList();
        }
    }
    [RelayCommand]
    public async Task OnDeleteAuthor(AuthorModel author)
    {
        try
        {
            if (author == null)
            {
                await Shell.Current.DisplayAlert("Fehler", "Authordaten fehlerhaft.", "Ok");
                return;
            }
            var result = await Shell.Current.DisplayActionSheet("Author löschen?", "Abbrechen", null, "Löschen");

            if (result == "Abbrechen")
                return;

            await dataService.OnDeleteAuthor(author.Id);
            await Shell.Current.DisplayAlert("Erfolg", "Author wurde erfolgreich gelöscht.", "Ok");
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Löschen des Authors", ex.Message, DateTime.Now, LogStatus.Error);
            return;
        }
        finally
        {
            if (RequestClose is not null)
            {
                await RequestClose.Invoke();
                RequestClose = null;
            }
            await OnLoadAuthorList();
        }
    }
    [RelayCommand]
    public async Task OnCancel()
    {
        if (RequestClose is not null)
        {
            await RequestClose.Invoke();
            RequestClose = null;
        }
    }
    [RelayCommand]
    public async Task OnShowSwipeHelp()
    {
        await Shell.Current.DisplayAlert("Hinweis zur Bedienung",
        "Du kannst einen Author nach links oder rechts wischen: \n\n" +
        "- Nach links: Bearbeiten\n" +
        "- Nach rechts: Löschen \n\n", "Ok");
    }
    private void OnClearFields()
    {
        AuthorFirstname = "";
        AuthorSurname = "";
    }
}

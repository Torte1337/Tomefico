using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tomefico.Enums;
using Tomefico.Message;
using Tomefico.Models;
using Tomefico.Service;
using Tomefico.Views.Popup;

namespace Tomefico.ViewModels;

public partial class AuthorViewModel : ObservableObject
{

    public Func<Task>? RequestClose { get; set; }
    private ObservableCollection<AuthorModel> authorList = new();
    public ObservableCollection<AuthorModel> AuthorList
    {
        get => authorList;
        set => SetProperty(ref authorList, value);
    }
    private readonly LogService logService;
    private readonly DataService dataService;
    private readonly IServiceProvider serviceProvider;

    public AuthorViewModel(LogService logService, DataService dataService, IServiceProvider serviceProvider)
    {
        this.logService = logService;
        this.dataService = dataService;
        this.serviceProvider = serviceProvider;
        WeakReferenceMessenger.Default.Unregister<MessageRefreshAuthorList>(this);
        WeakReferenceMessenger.Default.Register<MessageRefreshAuthorList>(this, OnReloadList);
        _ = OnLoadAuthorList();
    }
    
    [RelayCommand]
    public async Task OnShowPopupCreateAuthor()
    {
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
    public async Task OnAuthorButtonPressed(AuthorModel author)
    {
        var firstResult = await Shell.Current.DisplayActionSheet("Was möchtest du machen?", "Abbrechen", null, "Bearbeiten", "Löschen");
        if (!string.IsNullOrWhiteSpace(firstResult))
        {
            if (firstResult.Contains("Abbrechen"))
                return;
            else if (firstResult.Contains("Bearbeiten"))
            {
                await OnShowPopupEditAuthor(author);
            }
            else if (firstResult.Contains("Löschen"))
            {
                await OnDeleteAuthor(author);
            }
        }
    }

    
    private async Task OnShowPopupEditAuthor(AuthorModel author)
    {
        if (author == null)
        {
            await Shell.Current.DisplayAlert("Fehler", "Daten des Autors sind Fehlerhaft und können nicht geladen werden.", "Ok");
            return;
        }
        var popup = serviceProvider.GetRequiredService<AuthorCreateEditPopup>();
        WeakReferenceMessenger.Default.Send(new MessageEditAuthor(author));
        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
    }
    private async Task OnDeleteAuthor(AuthorModel author)
    {
        try
        {
            if (author == null)
            {
                await Shell.Current.DisplayAlert("Fehler", "Autordaten fehlerhaft.", "Ok");
                return;
            }
            var result = await Shell.Current.DisplayActionSheet("Autor löschen?", "Abbrechen", null, "Löschen");

            if (result == "Abbrechen")
                return;

            await dataService.OnDeleteAuthor(author.Id);
            await Shell.Current.DisplayAlert("Erfolg", "Autor wurde erfolgreich gelöscht.", "Ok");
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Löschen des Autors", ex.Message, DateTime.Now, LogStatus.Error);
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
    public async void OnReloadList(object recipient, MessageRefreshAuthorList msg)
    {
        await OnLoadAuthorList();
    }
    public async Task OnLoadAuthorList()
    {
        var authors = await dataService.OnGetAuthorList();
        if (authors == null)
        {
            await Shell.Current.DisplayAlert("Fehler", "Liste der Authoren konnte nicht geladen werden, bitte versuche es erneut.", "Ok");
            return;
        }
        if (AuthorList != null && AuthorList.Count > 0)
            AuthorList.Clear();
            
        AuthorList = new ObservableCollection<AuthorModel>(authors);
    }


}

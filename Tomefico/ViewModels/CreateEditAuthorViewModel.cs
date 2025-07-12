using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tomefico.Message;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.ViewModels;

public partial class CreateEditAuthorViewModel : ObservableObject
{
    [ObservableProperty] private string authorFirstname = "";
    [ObservableProperty] private string authorSurname = "";
    private readonly DataService dataService;
    private AuthorModel? editAuthor = null;
    public Func<Task>? RequestClose { get; set; }

    public CreateEditAuthorViewModel(DataService dataService)
    {
        this.dataService = dataService;
        WeakReferenceMessenger.Default.Unregister<MessageEditAuthor>(this);
        WeakReferenceMessenger.Default.Register<MessageEditAuthor>(this, OnSetFields);
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
            if (editAuthor == null)
            {
                var newAuthor = new AuthorModel
                {
                    Id = Guid.NewGuid(),
                    Firstname = AuthorFirstname,
                    Surname = AuthorSurname,
                    Name = AuthorFirstname + " " + AuthorSurname
                };

                var result = await dataService.OnAddAuthor(newAuthor);
                if (result)
                    await Shell.Current.DisplayAlert("Erfolg", "Autor wurde erfolgreich hinzugefügt.", "Ok");
                else
                    await Shell.Current.DisplayAlert("Fehler", "Autor wurde nicht erstellt.", "Ok");
            }
            else
            {
                var updatedAuthor = new AuthorUpdateModel();
                updatedAuthor.Id = editAuthor.Id;
                updatedAuthor.Firstname = AuthorFirstname;
                updatedAuthor.Surname = AuthorSurname;

                var result = await dataService.OnUpdateAuthor(updatedAuthor);
                if (result)
                    await Shell.Current.DisplayAlert("Erfolg", "Autor wurde erfolgreich aktualisiert.", "Ok");
                else
                    await Shell.Current.DisplayAlert("Fehler", "Autor wurde nicht aktualisiert.", "Ok");
            }

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erfolg", $"Fehler {ex.Message}", "Ok");
            return;
        }
        finally
        {
            WeakReferenceMessenger.Default.Send(new MessageRefreshAuthorList());
            OnClearFields();
            editAuthor = null;
            if (RequestClose is not null)
            {
                await RequestClose.Invoke();
                RequestClose = null;
            }
        }
    }
    private void OnClearFields()
    {
        AuthorFirstname = "";
        AuthorSurname = "";
    }
    public void OnSetFields(object recipient, MessageEditAuthor msg)
    {
        if (msg == null || msg.Value == null)
            return;


        editAuthor = msg.Value;
        AuthorFirstname = msg.Value.Firstname;
        AuthorSurname = msg.Value.Surname;
    }
}

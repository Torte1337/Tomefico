using System;
using Android.Service.Autofill;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Tomefico.Service;

namespace Tomefico.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly DataService dataService;
    private readonly IFileSaver fileSaver;
    private readonly PathService pathService;
    public SettingsViewModel(DataService dataService, IFileSaver fileSaver, PathService pathService)
    {
        this.dataService = dataService;
        this.fileSaver = fileSaver;
        this.pathService = pathService;
    }


    [RelayCommand]
    public async Task OnExportDatabase()
    {
        try
        {
            var stream = await dataService.GetDatabaseStreamAsync();
            if (stream is null)
            {
                await Shell.Current.DisplayAlert("Fehler", "Datenbank konnte nicht geladen werden.", "OK");
                return;
            }

            var result = await fileSaver.SaveAsync(
                "tomefico.db",
                "application/octet-stream",
                stream);

            if (result.IsSuccessful)
                await Shell.Current.DisplayAlert("Erfolg", "Datenbank exportiert.", "OK");
            else
                await Shell.Current.DisplayAlert("Fehler", "Export abgebrochen oder fehlgeschlagen.", "OK");                    
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler beim Export", ex.Message, "OK");
        }
    }
    [RelayCommand]
    public async Task OnImportDatabase()
    {
        try
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] {".db", ".sqlite"}},
                { DevicePlatform.iOS, new[] {".db",".sqlite"}}
            });
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Importiere Datenbank",
                FileTypes = customFileType
            });

            if (result != null)
            {
                var filePath = result.FullPath;
                var success = await dataService.OnImportDatabase(filePath);

                if (success)
                    await Shell.Current.DisplayAlert("Erfolg", "Datenbank erfolgreich importiert.", "OK");
                else
                    await Shell.Current.DisplayAlert("Fehler", "Import der Datenbank fehlgeschlagen.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", ex.Message, "Ok");
        }
    }
    [RelayCommand]
    public async Task OnResetDatabase()
    {
        if (await dataService.OnResetTheDatabase())
            await Shell.Current.DisplayAlert("Info", "Datenbank wurde erfolgreich zurückgesetzt", "Ok");
        else
            await Shell.Current.DisplayAlert("Fehler", "Datenbank wurde durch einen Fehler nicht zurückgesetzt.", "Ok");
    }
    [RelayCommand]
    public async Task OnGetInfos()
    {
        //Popup anzeigen mit Infos der App und von mir ?
    }
}

using System;
using Android.Service.Autofill;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Nio.FileNio;
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
            var dbPath = pathService.GetDatabasePath();
            using var sourceStream = new FileStream(dbPath, FileMode.Open, FileAccess.Read);

            var fileName = $"tomfico.db";
            var fileResult = await fileSaver.SaveAsync(fileName, sourceStream, default);

            if (fileResult.IsSuccessful)
                await Shell.Current.DisplayAlert("Erfolg", "Datenbank exportiert!", "Ok");
            else
                await Shell.Current.DisplayAlert("Fehler", "Speichern fehlgeschalgen", "Ok");            

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
            // 1. Backup-Datei wählen
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Pdf, // SQLite-Typen sind nicht vordefiniert
                PickerTitle = "Wähle eine Datenbank zum Importieren"
            });

            if (result == null) return;

            // 2. Temporären Pfad für die neue DB erstellen
            var tempPath = System.IO.Path.Combine(Microsoft.Maui.Storage.FileSystem.CacheDirectory, "tomefico.db");
            var targetPath = pathService.GetDatabasePath();

            // 3. Vorhandene DB sichern
            var backupPath = System.IO.Path.Combine(Microsoft.Maui.Storage.FileSystem.CacheDirectory, "tomefico.db");
            if (File.Exists(targetPath))
            {
                File.Copy(targetPath, backupPath, true);
                File.Delete(targetPath);
            }

            // 4. Neue Datei kopieren
            using var sourceStream = await result.OpenReadAsync();
            using var targetStream = File.Create(targetPath);
            await sourceStream.CopyToAsync(targetStream);

            // 5. EF Core Context neu laden
            

            await Shell.Current.DisplayAlert("Erfolg", "Datenbank importiert!", "OK");
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

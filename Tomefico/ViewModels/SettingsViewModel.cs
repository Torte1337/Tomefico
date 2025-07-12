using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Service;

namespace Tomefico.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly DataService dataService;
    public SettingsViewModel(DataService dataService)
    {
        this.dataService = dataService;
    }


    
    [RelayCommand]
    public async Task OnResetDatabase()
    {
        var result = await Shell.Current.DisplayActionSheet("Datenbank wirklich zurücksetzen?", "Abbrechen", null, "Zurücksetzen");
        if (result != null)
        {
            if (result.Contains("Abbrechen"))
                return;
            else if(result.Contains("Zurücksetzen"))
            {
                if (await dataService.OnResetTheDatabase())
                    await Shell.Current.DisplayAlert("Info", "Datenbank wurde erfolgreich zurückgesetzt", "Ok");
                else
                    await Shell.Current.DisplayAlert("Fehler", "Datenbank wurde durch einen Fehler nicht zurückgesetzt.", "Ok");
            }
        }

    }
    [RelayCommand]
    public async Task OnGetInfos()
    {
        await Shell.Current.DisplayAlert("Information", "\n\n" +
        "Version: 1.0 \n" +
        "Copyright © Torsten Fergens 2025" ,
        "Ok");
    }
}

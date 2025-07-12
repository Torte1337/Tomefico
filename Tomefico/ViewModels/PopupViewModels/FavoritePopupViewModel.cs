using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.ViewModels.PopupViewModels;

public partial class FavoritePopupViewModel : PopupViewModelBase
{
    [ObservableProperty] private ObservableCollection<BookModel> favoriteList = new();
    public Func<Task>? RequestClose { get; set; }

    public FavoritePopupViewModel(DataService dataService) : base(dataService)
    {
        _ = OnLoadList();
    }

    private async Task OnLoadList()
    {
        FavoriteList = new(await dataService.OnLoadFavorites());
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
}

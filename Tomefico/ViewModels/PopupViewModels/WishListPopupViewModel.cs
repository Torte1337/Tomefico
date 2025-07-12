using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.ViewModels.PopupViewModels;

public partial class WishListPopupViewModel : PopupViewModelBase
{
    [ObservableProperty] private ObservableCollection<BookModel> wishList = new();
    public Func<Task>? RequestClose { get; set; }

    public WishListPopupViewModel(DataService dataService) : base(dataService)
    {
        _ = OnLoadList();
    }

    private async Task OnLoadList()
    {
        WishList = new(await dataService.OnLoadWishList());
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

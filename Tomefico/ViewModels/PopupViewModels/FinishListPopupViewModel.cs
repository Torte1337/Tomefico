using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.ViewModels.PopupViewModels;

public partial class FinishListPopupViewModel : PopupViewModelBase
{
    [ObservableProperty] private ObservableCollection<BookModel> finishList = new();
    public Func<Task>? RequestClose { get; set; }

    public FinishListPopupViewModel(DataService dataService) : base(dataService)
    {
        _ = OnLoadList();
    }

    private async Task OnLoadList()
    {
        FinishList = new(await dataService.OnLoadFinishedList());
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

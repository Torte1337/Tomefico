using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.ViewModels.PopupViewModels;

public partial class ToReadListPopupViewModel : PopupViewModelBase
{
    [ObservableProperty] private ObservableCollection<BookModel> toReadList = new();
    public Func<Task>? RequestClose { get; set; }

    public ToReadListPopupViewModel(DataService dataService) : base(dataService)
    {
        _ = OnLoadList();
    }

    private async Task OnLoadList()
    {
        ToReadList = new(await dataService.OnLoadToReadList());
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

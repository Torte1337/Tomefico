using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.ViewModels.PopupViewModels;

public partial class ReadingListPopupViewModel : PopupViewModelBase
{
    [ObservableProperty] private ObservableCollection<BookModel> readingList = new();
    public Func<Task>? RequestClose { get; set; }

    public ReadingListPopupViewModel(DataService dataService) : base(dataService)
    {
        _ = OnLoadList();
    }

    private async Task OnLoadList()
    {
        ReadingList = new(await dataService.OnLoadReadingList());
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

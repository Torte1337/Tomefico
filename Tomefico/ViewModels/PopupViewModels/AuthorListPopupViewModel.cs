using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Models;
using Tomefico.Service;

namespace Tomefico.ViewModels.PopupViewModels;

public partial class AuthorListPopupViewModel : PopupViewModelBase
{
    [ObservableProperty] private ObservableCollection<AuthorModel> authorList = new();
    public Func<Task>? RequestClose { get; set; }

    public AuthorListPopupViewModel(DataService dataService) : base(dataService)
    {
        _ = OnLoadList();
    }
    private async Task OnLoadList()
    {
        AuthorList = new(await dataService.OnGetAuthorList());
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

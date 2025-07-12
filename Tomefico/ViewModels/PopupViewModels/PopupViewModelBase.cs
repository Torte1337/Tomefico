using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Tomefico.Service;

namespace Tomefico.ViewModels.PopupViewModels;

public abstract class PopupViewModelBase : ObservableObject
{
    protected readonly DataService dataService;
    public PopupViewModelBase(DataService dataService) => this.dataService = dataService;
}

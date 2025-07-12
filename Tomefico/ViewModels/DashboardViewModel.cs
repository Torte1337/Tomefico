using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tomefico.Models;
using Tomefico.Service;
using Tomefico.Views.Popup;

namespace Tomefico.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IServiceProvider serviceProvider;
    public DashboardViewModel(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;
    
    [RelayCommand]
    public async Task OpenWishlist()
    {
        var wishlistPopup = serviceProvider.GetRequiredService<WishListPopup>();
        await Shell.Current.CurrentPage.ShowPopupAsync(wishlistPopup);
    }
    [RelayCommand]
    public async Task OpenToReadList()
    {
        var toReadPopup = serviceProvider.GetRequiredService<ToReadListPopup>();
        await Shell.Current.CurrentPage.ShowPopupAsync(toReadPopup);
    }
    [RelayCommand]
    public async Task OpenReadingList()
    {
        var ReadingPopup = serviceProvider.GetRequiredService<ReadingListPopup>();
        await Shell.Current.CurrentPage.ShowPopupAsync(ReadingPopup);
    }
    [RelayCommand]
    public async Task OpenFinishedList()
    {
        var FinishPopup = serviceProvider.GetRequiredService<FinishListPopup>();
        await Shell.Current.CurrentPage.ShowPopupAsync(FinishPopup);
    }
    [RelayCommand]
    public async Task OpenFavoriteList()
    {
        var favoritePopup = serviceProvider.GetRequiredService<FavoriteListPopup>();
        await Shell.Current.CurrentPage.ShowPopupAsync(favoritePopup);
    }
    [RelayCommand]
    public async Task OpenAuthorList()
    {
        var authorPopup = serviceProvider.GetRequiredService<AuthorListPopup>();
        await Shell.Current.CurrentPage.ShowPopupAsync(authorPopup);
    }

}



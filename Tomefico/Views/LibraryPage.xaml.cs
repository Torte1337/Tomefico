using Tomefico.ViewModels;

namespace Tomefico.Views;

public partial class LibraryPage : ContentPage
{
	public LibraryPage(LibraryViewModel pm)
	{
		InitializeComponent();
		BindingContext = pm;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is LibraryViewModel libraryViewModel)
		{
			libraryViewModel.IsFirstLoad = true;
			_ = libraryViewModel.OnLoadLists();
		}
    }
}
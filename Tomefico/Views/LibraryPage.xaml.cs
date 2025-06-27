using Tomefico.ViewModels;

namespace Tomefico.Views;

public partial class LibraryPage : ContentPage
{
	public LibraryPage(LibraryViewModel pm)
	{
		InitializeComponent();
		BindingContext = pm;
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is LibraryViewModel vm)
			await vm.OnLoadBookList();
	}
}
using Tomefico.ViewModels;

namespace Tomefico.Views;

public partial class LibraryPage : ContentPage
{
	public LibraryPage(LibraryViewModel pm)
	{
		InitializeComponent();
		BindingContext = pm;
	}
}
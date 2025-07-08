using Tomefico.ViewModels;

namespace Tomefico.Views;

public partial class AuthorPage : ContentPage
{
	public AuthorPage(AuthorViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
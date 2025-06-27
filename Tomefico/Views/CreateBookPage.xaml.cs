using Tomefico.ViewModels;

namespace Tomefico.Views;

public partial class CreateBookPage : ContentPage
{
	public CreateBookPage(CreateBookViewModel pm)
	{
		InitializeComponent();
		BindingContext = pm;
	}
}
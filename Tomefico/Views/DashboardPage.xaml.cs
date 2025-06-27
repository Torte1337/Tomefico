using Tomefico.ViewModels;

namespace Tomefico.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel pm)
	{
		InitializeComponent();
		BindingContext = pm;
	}
}
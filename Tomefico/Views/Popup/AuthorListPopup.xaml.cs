using Tomefico.ViewModels;

namespace Tomefico.Views.Popup;

public partial class AuthorListPopup : CommunityToolkit.Maui.Views.Popup
{
	public AuthorListPopup(DashboardViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
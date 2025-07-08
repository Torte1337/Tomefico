using Tomefico.ViewModels;

namespace Tomefico.Views.Popup;

public partial class AuthorCreateEditPopup : CommunityToolkit.Maui.Views.Popup
{
	public AuthorCreateEditPopup(AuthorViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
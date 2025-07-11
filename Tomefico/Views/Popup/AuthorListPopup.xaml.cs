using Tomefico.ViewModels;
using Tomefico.ViewModels.PopupViewModels;

namespace Tomefico.Views.Popup;

public partial class AuthorListPopup : CommunityToolkit.Maui.Views.Popup
{
	public AuthorListPopup(AuthorListPopupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
using Tomefico.ViewModels.PopupViewModels;

namespace Tomefico.Views.Popup;

public partial class WishListPopup : CommunityToolkit.Maui.Views.Popup
{
	public WishListPopup(WishListPopupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
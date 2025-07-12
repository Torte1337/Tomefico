using Tomefico.ViewModels.PopupViewModels;

namespace Tomefico.Views.Popup;

public partial class FavoriteListPopup : CommunityToolkit.Maui.Views.Popup
{
	public FavoriteListPopup(FavoritePopupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
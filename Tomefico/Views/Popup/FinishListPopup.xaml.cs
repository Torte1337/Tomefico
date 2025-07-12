using Tomefico.ViewModels.PopupViewModels;

namespace Tomefico.Views.Popup;

public partial class FinishListPopup : CommunityToolkit.Maui.Views.Popup
{
	public FinishListPopup(FinishListPopupViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };		
	}
}
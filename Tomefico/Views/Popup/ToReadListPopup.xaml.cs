using Tomefico.ViewModels.PopupViewModels;

namespace Tomefico.Views.Popup;

public partial class ToReadListPopup : CommunityToolkit.Maui.Views.Popup
{
	public ToReadListPopup(ToReadListPopupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };		
	}
}
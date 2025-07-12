using Tomefico.ViewModels.PopupViewModels;

namespace Tomefico.Views.Popup;

public partial class ReadingListPopup : CommunityToolkit.Maui.Views.Popup
{
	public ReadingListPopup(ReadingListPopupViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };		
	}
}
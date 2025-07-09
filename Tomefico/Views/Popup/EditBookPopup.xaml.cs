using Tomefico.ViewModels;

namespace Tomefico.Views.Popup;

public partial class EditBookPopup : CommunityToolkit.Maui.Views.Popup
{
	public EditBookPopup(EditBookViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;


		vm.OnRegisterMessage();
		vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
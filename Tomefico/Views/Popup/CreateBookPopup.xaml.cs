using Tomefico.ViewModels.PopupViewModels;

namespace Tomefico.Views.Popup;

public partial class CreateBookPopup : CommunityToolkit.Maui.Views.Popup
{
	public CreateBookPopup(CreateBookViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
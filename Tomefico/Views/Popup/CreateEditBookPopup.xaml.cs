using Tomefico.ViewModels;

namespace Tomefico.Views.Popup;

public partial class CreateEditBookPopup : CommunityToolkit.Maui.Views.Popup
{
	public CreateEditBookPopup(CreateBookViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
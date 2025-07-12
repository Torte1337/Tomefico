using Tomefico.ViewModels;

namespace Tomefico.Views.Popup;

public partial class AuthorCreateEditPopup : CommunityToolkit.Maui.Views.Popup
{
	public AuthorCreateEditPopup(CreateEditAuthorViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
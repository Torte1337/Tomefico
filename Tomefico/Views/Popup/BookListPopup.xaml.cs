using Tomefico.ViewModels;

namespace Tomefico.Views.Popup;

public partial class BookListPopup : CommunityToolkit.Maui.Views.Popup
{
	public BookListPopup(DashboardViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
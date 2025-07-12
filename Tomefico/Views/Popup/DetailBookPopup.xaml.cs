using Tomefico.Models;
using Tomefico.ViewModels;

namespace Tomefico.Views.Popup;

public partial class DetailBookPopup : CommunityToolkit.Maui.Views.Popup
{
	public DetailBookPopup(DetailBookViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		if(vm != null)
			vm.RequestClose = async () => { await this.CloseAsync(); };
	}
}
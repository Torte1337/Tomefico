using Tomefico.Models;
using Tomefico.ViewModels;

namespace Tomefico.Views.Popup;

public partial class DetailBookPopup : CommunityToolkit.Maui.Views.Popup
{
	public DetailBookPopup(BookModel model)
	{
		InitializeComponent();
		BindingContext = new DetailBookViewModel(model);

		var ctx = BindingContext as DetailBookViewModel;
		if(ctx != null)
			ctx.RequestClose = async () => { await this.CloseAsync(); };
	}
}
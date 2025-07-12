using Tomefico.ViewModels;

namespace Tomefico.Views;

public partial class AuthorPage : ContentPage
{
	public AuthorPage(AuthorViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is AuthorViewModel vm)
		{
			await Task.Delay(100);
			var oldList = vm.AuthorList.ToList();
			vm.AuthorList.Clear();
			foreach (var author in oldList)
			{
				vm.AuthorList.Add(author);
			}
		}
	}
}
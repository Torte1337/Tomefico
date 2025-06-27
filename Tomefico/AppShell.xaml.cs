using Tomefico.Views;

namespace Tomefico;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
		Routing.RegisterRoute(nameof(LibraryPage), typeof(LibraryPage));
	}
}

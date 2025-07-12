using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Tomefico.Service;
using Tomefico.Data;
using Microsoft.EntityFrameworkCore;
using Tomefico.ViewModels;
using Tomefico.Views;
using Tomefico.Views.Popup;
using Tomefico.ViewModels.PopupViewModels;
using Tomefico.Converter;

namespace Tomefico;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddAppServices();
		builder.Services.AddViewModels();
		builder.Services.AddPages();	

		builder.Logging.AddDebug();

		return builder.Build();
	}
	private static void AddAppServices(this IServiceCollection services)
	{
		services.AddDbContext<TomeContext>(options =>
		{
			options.UseSqlite(PathService.GetSQLiteConnectionString());
		});

		services.AddSingleton<LogService>();
		services.AddSingleton<DataService>();
		services.AddSingleton<ByteArrayToImageSourceConverter>();
	}
	private static void AddViewModels(this IServiceCollection services)
	{
		services.AddTransient<DashboardViewModel>();
		services.AddTransient<LibraryViewModel>();
		services.AddTransient<AuthorViewModel>();
		services.AddTransient<EditBookViewModel>();
		services.AddTransient<CreateBookViewModel>();
		services.AddTransient<DetailBookViewModel>();
		services.AddTransient<SettingsViewModel>();
		services.AddTransient<DetailBookViewModel>();
		services.AddTransient<CreateEditAuthorViewModel>();
		services.AddTransient<AuthorListPopupViewModel>();
		services.AddTransient<FavoritePopupViewModel>();
		services.AddTransient<WishListPopupViewModel>();
		services.AddTransient<ToReadListPopupViewModel>();
		services.AddTransient<ReadingListPopupViewModel>();
		services.AddTransient<FinishListPopupViewModel>();
	}
	private static void AddPages(this IServiceCollection services)
	{
		services.AddTransient<DashboardPage>();
		services.AddTransient<LibraryPage>();
		services.AddTransient<CreateBookPopup>();
		services.AddTransient<DetailBookPopup>();
		services.AddTransient<AuthorListPopup>();
		services.AddTransient<SettingsPage>();
		services.AddTransient<AuthorPage>();
		services.AddTransient<AuthorCreateEditPopup>();
		services.AddTransient<EditBookPopup>();
		services.AddTransient<FavoriteListPopup>();
		services.AddTransient<WishListPopup>();
		services.AddTransient<ToReadListPopup>();
		services.AddTransient<ReadingListPopup>();
		services.AddTransient<FinishListPopup>();
	}
}

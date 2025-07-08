using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Tomefico.Service;
using Tomefico.Data;
using Microsoft.EntityFrameworkCore;
using Tomefico.ViewModels;
using Tomefico.Views;
using Tomefico.Views.Popup;
using CommunityToolkit.Maui.Storage;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
	private static void AddAppServices(this IServiceCollection services)
	{
		services.AddSingleton<PathService>();
		services.AddSingleton<IFileSaver>(FileSaver.Default);

		services.AddDbContext<TomeContext>((serviceProvider, options) =>
		{
			var pathService = serviceProvider.GetRequiredService<PathService>();
			options.UseSqlite(pathService.GetSQLiteConnectionString());
		});

		services.AddSingleton<DataService>();
		services.AddSingleton<LogService>();
	}
	private static void AddViewModels(this IServiceCollection services)
	{
		services.AddScoped<DashboardViewModel>();
		services.AddScoped<LibraryViewModel>();
		services.AddScoped<CreateBookViewModel>();
		services.AddScoped<DetailBookViewModel>();
		services.AddScoped<SettingsViewModel>();
		services.AddScoped<AuthorViewModel>();
	}
	private static void AddPages(this IServiceCollection services)
	{
		services.AddTransient<DashboardPage>();
		services.AddTransient<LibraryPage>();
		services.AddTransient<CreateEditBookPopup>();
		services.AddTransient<DetailBookPopup>();
		services.AddTransient<BookListPopup>();
		services.AddTransient<AuthorListPopup>();
		services.AddTransient<SettingsPage>();
		services.AddTransient<AuthorPage>();
		services.AddTransient<AuthorCreateEditPopup>();
	}
}

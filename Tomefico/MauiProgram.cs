using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Tomefico.Service;
using Tomefico.Data;
using Microsoft.EntityFrameworkCore;
using Tomefico.ViewModels;
using Tomefico.Views;
using Tomefico.Views.Popup;

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

		builder.Services.AddSingleton<PathService>();

		builder.Services.AddDbContext<TomeContext>((serviceProvider, options) =>
		{
			var pathService = serviceProvider.GetRequiredService<PathService>();
			options.UseSqlite(pathService.GetSQLiteConnectionString());
		});

		builder.Services.AddSingleton<DataService>();
		builder.Services.AddSingleton<LogService>();
		builder.Services.AddScoped<DashboardViewModel>();
		builder.Services.AddScoped<LibraryViewModel>();
		builder.Services.AddScoped<CreateBookViewModel>();
		builder.Services.AddScoped<DetailBookViewModel>();
		builder.Services.AddScoped<SettingsViewModel>();

		builder.Services.AddTransient<DashboardPage>();
		builder.Services.AddTransient<LibraryPage>();
		builder.Services.AddTransient<CreateEditBookPopup>();
		builder.Services.AddTransient<DetailBookPopup>();
		builder.Services.AddTransient<BookListPopup>();
		builder.Services.AddTransient<AuthorListPopup>();
		builder.Services.AddTransient<SettingsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

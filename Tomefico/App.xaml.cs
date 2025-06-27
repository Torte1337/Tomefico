using Tomefico.Data;

namespace Tomefico;

public partial class App : Application
{
	public App(TomeContext tomeContext)
	{
		InitializeComponent();

		tomeContext.Database.EnsureCreated();
#if ANDROID
		if (DeviceInfo.Current.Platform == DevicePlatform.Android)
		{
			Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
			{
				var platformView = handler.PlatformView;
				platformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
			});
		}
		if (DeviceInfo.Current.Platform == DevicePlatform.Android)
		{
			Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
			{
				var platformView = handler.PlatformView;
				platformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
			});
		}
		if (DeviceInfo.Current.Platform == DevicePlatform.Android)
		{
			Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping(nameof(DatePicker), (handler, view) =>
			{
				var platformView = handler.PlatformView;
				platformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
			});
		}
		if (DeviceInfo.Current.Platform == DevicePlatform.Android)
		{
			Microsoft.Maui.Handlers.SearchBarHandler.Mapper.AppendToMapping(nameof(SearchBar), (handler, view) =>
			{
				var platformView = handler.PlatformView;
				platformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
			});
		}
		if(DeviceInfo.Current.Platform == DevicePlatform.Android)
		{
			Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(Editor), (handler, view) =>
			{
				var platformView = handler.PlatformView;
				platformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
			});
		}
#endif
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
	
}
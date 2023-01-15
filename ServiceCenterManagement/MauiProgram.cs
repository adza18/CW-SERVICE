using Microsoft.AspNetCore.Components.WebView.Maui;
using ServiceCenterManagement.Data;
using MudBlazor.Services;

namespace ServiceCenterManagement;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		builder.Services.AddMudServices();



        builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddSingleton<RecordService>();
        builder.Services.AddSingleton<ItemService>();
        builder.Services.AddSingleton<UtilService>();
        return builder.Build();
	}
}

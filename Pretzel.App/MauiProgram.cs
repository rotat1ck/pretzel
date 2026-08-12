using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
using Pretzel.App.ViewModels;
using Pretzel.Infrastructure;

namespace Pretzel.App;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Lexend-VariableFont.ttf", "Lexend");
            })
            .ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
                events.AddWindows(windowsBuilder =>
                {
                    windowsBuilder.OnWindowCreated(window =>
                    {
                        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

                        if (appWindow is not null)
                        {
                            var titleBar = appWindow.TitleBar;
                            titleBar.ButtonForegroundColor = Colors.White.ToWindowsColor();
                            titleBar.ButtonInactiveForegroundColor = Colors.White.ToWindowsColor();

                            titleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
                        }
                    });
                });
#endif
            })
            .Services.AddTransient<TestViewModel>()
            .AddInfrastructure();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

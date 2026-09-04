using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
using Pretzel.App.ViewModels;
using Pretzel.App.WinUI;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models.Search;
using Pretzel.Infrastructure;
using Pretzel.Infrastructure.Services.Search;

namespace Pretzel.App;

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
            });

        builder.Services.AddTransient<ExploreViewModel>();
        builder.Services.AddTransient<SearchSessionViewModel>();


        builder.Services.AddInfrastructure();
        builder.Services.RegisterSettingProviders(FileSystem.AppDataDirectory);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    
}

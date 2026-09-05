using Microsoft.Extensions.DependencyInjection;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Download;
using Pretzel.Core.Models.Search;
using Pretzel.Infrastructure.Services.Download;
using Pretzel.Infrastructure.Services.Providers;
using Pretzel.Infrastructure.Services.Search;

namespace Pretzel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(DependencyInjection).Assembly));

        services.AddHttpClient(ChartSource.ChorusEncore.ToString(), client =>
        {
            client.BaseAddress = ChartSource.ChorusEncore.ResolveBaseUri().WithSubdomain("api");
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        services.AddHttpClient(ChartSource.ChorusEncore.ResolveBaseUri().WithSubdomain("files").ToString(), client =>
        {
            client.BaseAddress = ChartSource.ChorusEncore.ResolveBaseUri().WithSubdomain("files");
            client.Timeout = TimeSpan.FromMinutes(90);
        });

        services.AddHttpClient(ChartSource.RhythmVerse.ToString(), client =>
        {
            client.BaseAddress = ChartSource.RhythmVerse.ResolveBaseUri();
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        services.AddKeyedSingleton<IChartSearchStrategy, ChorusEncoreSearchStrategy>(ChartSource.ChorusEncore);
        services.AddKeyedSingleton<IChartSearchStrategy, RhythmVerseSearchStrategy>(ChartSource.RhythmVerse);

        services.AddSingleton<IChartSearchService, ChartSearchService>();

        services.AddKeyedSingleton<IChartDownloadStrategy, ChorusEncoreDownloadStrategy>(ChartSource.ChorusEncore);

        services.AddSingleton<IChartDownloadService, ChartDownloadService>();
        services.AddSingleton<IChartWriterService, ChartWriterService>();

        return services;
    }

    public static IServiceCollection RegisterSettingProviders(this IServiceCollection services, string appDataDirectory)
    {
        services.AddSingleton<ISettingProvider<ChartSearchSettings>>(provider => new SettingsProvider<ChartSearchSettings>(appDataDirectory, "search_settings.json"));
        services.AddSingleton<ISettingProvider<ChartDownloadSettings>>(provider => new SettingsProvider<ChartDownloadSettings>(appDataDirectory, "download_settings.json"));

        return services;
    }
}

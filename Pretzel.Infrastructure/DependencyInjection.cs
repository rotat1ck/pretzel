using Microsoft.Extensions.DependencyInjection;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces;
using Pretzel.Infrastructure.Services.Search;

namespace Pretzel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(DependencyInjection).Assembly));

        services.AddHttpClient(ChartSource.ChorusEncore.ToString(), client =>
        {
            client.BaseAddress = ChartSource.ChorusEncore.ResolveBaseUri();
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        services.AddHttpClient(ChartSource.RhythmVerse.ToString(), client =>
        {
            client.BaseAddress = ChartSource.RhythmVerse.ResolveBaseUri();
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        services.AddKeyedSingleton<IChartSearchStrategy, ChorusEncoreSearchStrategy>(ChartSource.ChorusEncore);
        services.AddKeyedSingleton<IChartSearchStrategy, RhythmVerseSearchStrategy>(ChartSource.RhythmVerse);

        services.AddSingleton<IChartSearchService, ChartSearchService>();

        return services;
    }
}

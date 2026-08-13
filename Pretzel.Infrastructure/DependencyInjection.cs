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
            client.BaseAddress = new Uri("https://api.enchor.us");
        });

        services.AddHttpClient(ChartSource.RhythmVerse.ToString(), client =>
        {
            client.BaseAddress = new Uri("https://rhythmverse.co");
        });

        services.AddKeyedSingleton<IChartSearchStrategy, ChorusEncoreSearchStrategy>(ChartSource.ChorusEncore);

        return services;
    }
}

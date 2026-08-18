using Microsoft.Extensions.DependencyInjection;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models.Search;

namespace Pretzel.Infrastructure.Services.Search;

public class ChartSearchService : IChartSearchService
{
    private ISettingProvider<ChartSearchSettings> searchSettingProvider;
    private IServiceProvider serviceProvider;

    public ChartSearchService(ISettingProvider<ChartSearchSettings> searchSettingProvider, IServiceProvider serviceProvider)
    {
        this.searchSettingProvider = searchSettingProvider;
        this.serviceProvider = serviceProvider;
    }

    public async IAsyncEnumerable<ChartSearchResults> SearchAsync(ChartSearchOptions searchOptions, CancellationTokenSource cts)
    {
        var settings = searchSettingProvider.GetValue();

        var strategies = settings.EnabledSources.Select(key => serviceProvider.GetRequiredKeyedService<IChartSearchStrategy>(key)).ToList();
        if (!settings.ContinueSearchWithUnsupportedOptions)
        {
            strategies = strategies.Where(strategy => strategy.CanHandle(searchOptions)).ToList();
        }

        if (strategies.Count == 0)
        {
            yield break;
        }

        var cancellationToken = cts?.Token ?? CancellationToken.None;
        var taskDict = strategies.ToDictionary(
            strategy => strategy.SearchAsync(searchOptions, cancellationToken),
            strategy => strategy
        );

        await foreach (var completedTask in Task.WhenEach(taskDict.Keys).WithCancellation(cancellationToken))
        {
            var strategy = taskDict[completedTask];

            ChartSearchResults searchResult;
            try
            {
                searchResult = await completedTask;
                searchResult.ChartSource = strategy.Source;
            }
            catch (Exception ex)
            {
                searchResult = new ChartSearchResults
                {
                    ChartSource = strategy.Source,
                    Items = [],
                    Count = 0,
                    Returned = 0,
                    ProblemDetails = new()
                    {
                        ExceptionMessage = ex.Message,
                        ExceptionName = ex.GetType().Name
                    }
                };
            }

            yield return searchResult;
        }
    }
}

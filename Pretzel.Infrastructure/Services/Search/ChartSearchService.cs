using Microsoft.Extensions.DependencyInjection;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models.Search;

namespace Pretzel.Infrastructure.Services.Search;

public class ChartSearchService : IChartSearchService
{
    private ISettingProvider<ChartSearchSettings> searchSettingProvider;
    private IServiceProvider serviceProvider;
    private ChartSearchSettings searchSettings;

    public ChartSearchService(ISettingProvider<ChartSearchSettings> searchSettingProvider, IServiceProvider serviceProvider)
    {
        this.searchSettingProvider = searchSettingProvider;
        this.searchSettings = searchSettingProvider.GetValue();
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
            strategy => strategy.SearchAsync(searchOptions),
            strategy => strategy
        );

        while (taskDict.Count > 0)
        {
            var completedTask = await Task.WhenAny(taskDict.Keys);

            var strategy = taskDict[completedTask];
            taskDict.Remove(completedTask);

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

            cancellationToken.ThrowIfCancellationRequested();

            yield return searchResult;
        }

    }
}

using Pretzel.Core.Interfaces;
using Pretzel.Core.Models.Search;

namespace Pretzel.Infrastructure.Services.Search;

public class ChartSearchService : IChartSearchService
{
    private ISettingProvider<ChartSearchSettings> searchSettingProvider;
    private ChartSearchSettings searchSettings;

    public ChartSearchService(ISettingProvider<ChartSearchSettings> searchSettingProvider)
    {
        this.searchSettingProvider = searchSettingProvider;
        this.searchSettings = searchSettingProvider.GetValue();
    }

    public async Task<IAsyncEnumerable<ChartSearchResults>> SearchAsync(ChartSearchOptions searchOptions, CancellationTokenSource cts)
    {
        throw new NotImplementedException();
    }
}

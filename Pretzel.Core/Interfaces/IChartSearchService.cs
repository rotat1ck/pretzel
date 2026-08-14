using Pretzel.Core.Models.Search;

namespace Pretzel.Core.Interfaces;

public interface IChartSearchService
{
    Task<IAsyncEnumerable<ChartSearchResults>> SearchAsync(ChartSearchOptions searchOptions, CancellationTokenSource cts);
}

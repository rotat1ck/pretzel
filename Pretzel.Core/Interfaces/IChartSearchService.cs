using Pretzel.Core.Models.Search;

namespace Pretzel.Core.Interfaces;

public interface IChartSearchService
{
    IAsyncEnumerable<ChartSearchResults> SearchAsync(ChartSearchOptions searchOptions, CancellationToken cancellationToken = default);
}

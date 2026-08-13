using Pretzel.Core.Enums;
using Pretzel.Core.Models;
using Pretzel.Core.Models.Search;

namespace Pretzel.Core.Interfaces;

public interface IChartSearchStrategy
{
    ChartSource Source { get; }
    bool CanHandle(ChartSearchOptions options);
    Task<IEnumerable<Chart>> SearchAsync(ChartSearchOptions options);
}

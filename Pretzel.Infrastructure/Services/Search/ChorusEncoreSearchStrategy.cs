using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models;

namespace Pretzel.Infrastructure.Services.Search;

public class ChorusEncoreSearchStrategy : IChartSearchStrategy
{
    public ChartSource Source => ChartSource.ChorusEncore;

    public bool CanHandle(ChartSearchOptions options)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Chart>> SearchAsync(ChartSearchOptions options)
    {
        throw new NotImplementedException();
    }
}

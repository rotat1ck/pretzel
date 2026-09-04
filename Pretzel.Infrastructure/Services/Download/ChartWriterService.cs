using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;

namespace Pretzel.Infrastructure.Services.Download;

public class ChartWriterService : IChartWriterService
{
    public Task<DownloadStatus> TryWriteChartAsync(DownloadResult downloadResult, Chart chartInfo, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        try
        {

        }
        catch
        {

        }
    }
}

using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;

namespace Pretzel.Infrastructure.Services.Download;

public class ChorusEncoreDownloadStrategy(IHttpClientFactory clientFactory) : IChartDownloadStrategy
{
    public ChartSource Source => ChartSource.ChorusEncore;

    public async Task<DownloadResult?> DownloadAsync(ChartDownloadSource source, CancellationToken cancellationToken)
    {
        var client = clientFactory.CreateClient(Source.ResolveBaseUri()
                                                      .WithSubdomain("files")
                                                      .ToString());

        var response = await client.GetAsync($"{source.ChartUri}.sng", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var stream = await response.Content.ReadAsStreamAsync();

        return new DownloadResult(response, stream);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Chart.SourceExtensions;
using Pretzel.Core.Models.Download;

namespace Pretzel.Infrastructure.Services.Download;

public class RhythmVerseDownloadStrategy(IHttpClientFactory clientFactory) : IChartDownloadStrategy
{
    private readonly IHttpClientFactory clientFactory = clientFactory;

    public ChartSource Source => ChartSource.RhythmVerse;

    public async Task<DownloadResult?> DownloadAsync(ChartDownloadSource source, CancellationToken cancellationToken)
    {
        if (source.ExtensionData is RhythmVerseChartSourceExtensionData extensionData
            && extensionData.IsExternal)
        {
            return default;
        }

        var client = clientFactory.CreateClient(Source.ResolveBaseUri()
                                                      .ToString());

        var response = await client.GetAsync($"{source.ChartUri}", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var stream = await response.Content.ReadAsStreamAsync();

        return new DownloadResult(response, stream) { FileType = DownloadFileType.Zip };
    }
}

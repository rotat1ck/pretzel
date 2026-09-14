using System;
using System.Collections.Generic;
using System.Text;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Download;

namespace Pretzel.Infrastructure.Services.Writer;

public class SngWriterStrategy : IChartWriterStrategy
{
    public DownloadFileType FileType => DownloadFileType.Sng;

    public Task WriteChartAsync(DownloadResult downloadResult, string targetDirectory, string chartName, CancellationToken cancellationToken)
    {
        // write .sng from stream into appdata with name = chartName

        // set output folder for SngDecode to target directory

        // call SngFileFormat.SngDecoder.DecodeSong with Path.Combine(appdata, chartName)

        throw new NotImplementedException();
    }
}

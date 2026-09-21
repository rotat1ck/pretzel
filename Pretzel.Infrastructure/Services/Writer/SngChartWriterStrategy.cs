using System;
using System.Collections.Generic;
using System.Text;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Download;
using SngFileFormat;

namespace Pretzel.Infrastructure.Services.Writer;

public class SngChartWriterStrategy : IChartWriterStrategy
{
    public DownloadFileType FileType => DownloadFileType.Sng;

    public async Task WriteChartAsync(DownloadResult downloadResult, string targetDirectory, string chartName, CancellationToken cancellationToken)
    {
        if (downloadResult.Stream is null)
        {
            throw new InvalidOperationException($"Chart stream is null");
        }

        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        var filePath = Path.Combine(targetDirectory, chartName + ".sng");
        using (FileStream fs = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await downloadResult.Stream.CopyToAsync(fs, cancellationToken);
        }

        await SngDecoder.DecodeSong(filePath);
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}

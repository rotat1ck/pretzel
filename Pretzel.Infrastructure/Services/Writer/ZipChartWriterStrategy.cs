using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Download;
using System.IO.Compression;

namespace Pretzel.Infrastructure.Services.Writer;

public class ZipChartWriterStrategy : IChartWriterStrategy
{
    public DownloadFileType FileType => DownloadFileType.Zip;

    public async Task WriteChartAsync(DownloadResult downloadResult, string targetDirectory, string chartName, CancellationToken cancellationToken)
    {
        if (downloadResult.Stream is null)
        {
            throw new InvalidOperationException($"Chart stream is null");
        }

        var chartFolder = Path.Combine(targetDirectory, chartName);
        if (!Directory.Exists(chartFolder))
        {
            Directory.CreateDirectory(chartFolder);
        }

        var tempZipFileName = chartFolder + ".zip";

        using (FileStream zipFs = new(tempZipFileName, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await downloadResult.Stream.CopyToAsync(zipFs, cancellationToken);
        }

        using FileStream tempFileFs = new(tempZipFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        using ZipArchive archive = new(tempFileFs, ZipArchiveMode.Read);

        foreach (var entry in archive.Entries)
        {
            if (string.IsNullOrEmpty(entry.Name))
            {
                continue;
            }

            var filePath = Path.Combine(chartFolder, entry.Name);

            var stream = await entry.OpenAsync(cancellationToken);
            using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fs, cancellationToken);
        }
    }
}

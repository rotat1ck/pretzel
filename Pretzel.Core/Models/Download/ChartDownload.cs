using Pretzel.Core.Enums;

namespace Pretzel.Core.Models.Download;

public class ChartDownload : IDisposable
{
    public DownloadStatus Status { get; set; } = DownloadStatus.Starting;
    public CancellationTokenSource Cts { get; set; } = new();
    public DownloadResult Result { get; set; } = null!;


    private bool disposed = false;

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        if (Cts is not null)
        {
            Cts.Cancel();
            Cts.Dispose();
        }

        disposed = true;
    }
}

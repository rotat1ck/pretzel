using Pretzel.Core.Enums;

namespace Pretzel.Core.Models.Download;

public class ChartDownload : IAsyncDisposable
{
    public DownloadStatus Status { get; set; } = DownloadStatus.None;
    public CancellationTokenSource Cts { get; init; } = new();
    public DownloadResult? Result { get; set; }


    private bool disposed = false;

    public async ValueTask DisposeAsync()
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

        if (Result is not null)
        {
            await Result.DisposeAsync();
        }

        disposed = true;
    }
}

using Pretzel.Core.Enums;

namespace Pretzel.Core.Models.Download;

public class DownloadResult : IAsyncDisposable
{
    private readonly HttpResponseMessage? response;
    private bool disposed = false;

    public Stream? Stream { get; }
    public long? TotalBytes { get; }
    public DownloadStatus Status { get; set; } = DownloadStatus.Starting;
    public DownloadFileType FileType { get; set; }
    public CancellationTokenSource? Cts { get; set; }

    public DownloadResult() { }

    public DownloadResult(HttpResponseMessage response, Stream stream)
    {
        this.response = response;

        TotalBytes = response.Content.Headers.ContentLength;
        Stream = stream;
    }

    public async ValueTask FreeUnmanagedResourcesAsync()
    {
        if (disposed)
        {
            return;
        }

        if (Stream is not null)
        {
            await Stream.DisposeAsync();
        }

        if (response is not null)
        {
            response.Dispose();
        }

        if (Cts is not null)
        {
            Cts.Cancel();
            Cts.Dispose();
        }

        disposed = true;
    }

    public async ValueTask DisposeAsync()
    {
        await FreeUnmanagedResourcesAsync();
    }
}

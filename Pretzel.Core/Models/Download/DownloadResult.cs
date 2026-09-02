using Pretzel.Core.Enums;

namespace Pretzel.Core.Models.Download;

public class DownloadResult : IAsyncDisposable
{
    private readonly HttpResponseMessage? response;

    public Stream? Stream { get; }
    public long? TotalBytes { get; }
    public DownloadStatus Status { get; set; } = DownloadStatus.Starting;
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
        if (Stream is not null)
        {
            await Stream.DisposeAsync();
        }

        if (response is not null)
        {
            response.Dispose();
        }

        Cts.Cancel();
        Cts.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await FreeUnmanagedResourcesAsync();
    }
}

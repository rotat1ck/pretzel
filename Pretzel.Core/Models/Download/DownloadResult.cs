namespace Pretzel.Core.Models.Download;

public class DownloadResult : IAsyncDisposable
{
    private readonly HttpResponseMessage response;

    public Stream? Stream { get; }
    public long? TotalBytes { get; }
    public string? ContentType { get; }
    public string? FileName { get; set; }

    public DownloadResult(HttpResponseMessage response, Stream stream)
    {
        this.response = response;

        TotalBytes = response.Content.Headers.ContentLength;
        ContentType = response.Content.Headers.ContentType?.MediaType;
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
    }

    public async ValueTask DisposeAsync()
    {
        await FreeUnmanagedResourcesAsync();
    }
}

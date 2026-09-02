namespace Pretzel.Core.Enums;

[Flags]
public enum DownloadStatus
{
    Starting = 0,
    Downloading = 1 << 0,
    Writing = 1 << 1,
    Finished = 1 << 2,
    Cancelled = 1 << 3,
    Failed = 1 << 4
}

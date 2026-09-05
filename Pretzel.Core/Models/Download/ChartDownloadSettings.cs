namespace Pretzel.Core.Models.Download;

public class ChartDownloadSettings : ICloneable
{
    public List<string> AvailableDirectories { get; set; } = [];
    public int? SelectedDirectory { get; set; }
    public string FileNamePattern { get; } = "{artist} - {name} ({charter})";

    public object Clone()
    {
        var clone = (ChartDownloadSettings)this.MemberwiseClone();
        clone.AvailableDirectories = new(this.AvailableDirectories);

        return clone;
    }
}

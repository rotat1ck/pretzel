using Pretzel.Core.Enums;

namespace Pretzel.Core.Models.Search;

public class ChartSearchSettings : ICloneable
{
    public bool ContinueSearchWithUnsupportedOptions { get; set; } = true;
    public List<ChartSource> EnabledSources { get; set; } = [ChartSource.ChorusEncore, ChartSource.RhythmVerse];

    public object Clone()
    {
        var clone = (ChartSearchSettings)this.MemberwiseClone();
        clone.EnabledSources = new(this.EnabledSources);

        return clone;
    }
}

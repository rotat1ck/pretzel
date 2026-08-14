using Pretzel.Core.Enums;

namespace Pretzel.Core.Models.Search;

public class ChartSearchSettings
{
    public bool ContinueSearchWithUnsupportedOptions { get; set; } = true;
    public List<ChartSource> EnabledSources { get; set; } = [ChartSource.ChorusEncore, ChartSource.RhythmVerse];

    public ChartSearchSettings() { }

    public ChartSearchSettings(ChartSearchSettings other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        EnabledSources = other.EnabledSources;
        ContinueSearchWithUnsupportedOptions = other.ContinueSearchWithUnsupportedOptions;
    }
}

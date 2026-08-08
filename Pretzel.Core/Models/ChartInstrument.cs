using Pretzel.Core.Enums;

namespace Pretzel.Core.Models;

public class ChartInstrument
{
    public InstrumentType Instrument { get; set; }
    public int Rating { get; set; }
    public HashSet<DifficultyLevel> AvailableDifficulties { get; set; } = new();
}

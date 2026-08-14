namespace Pretzel.Infrastructure.DTOs.Search.RhythmVerse;

public class RhythmVerseSearchRequest
{
    public int Page { get; set; }
    public int Records { get; set; }
    public string DataType => "full";

    public string? Text { get; set; }

    public string? Artist { get; set; } // requires a separate uri if isn't paired with "text"
    public string? Album { get; set; } // requires a separate uri if isn't paired with "text"
    public string? Author { get; set; } // requires a separate uri if isn't paired with "text"
    public string? Genre { get; set; } // requires a separate uri if isn't paired with "text"
    public int? Year { get; set; } // requires a separate uri if isn't paired with "text"
}

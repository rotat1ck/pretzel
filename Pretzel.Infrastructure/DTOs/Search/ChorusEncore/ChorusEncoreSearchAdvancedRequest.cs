namespace Pretzel.Infrastructure.DTOs.Search.ChorusEncore;

public class ChorusEncoreSearchAdvancedRequest : ChorusEncoreSearchRequest
{
    public required ChorusEncoreSearchFilter Name { get; set; }
    public required ChorusEncoreSearchFilter Artist { get; set; }
    public required ChorusEncoreSearchFilter Album { get; set; }
    public required ChorusEncoreSearchFilter Charter { get; set; }
    public required ChorusEncoreSearchFilter Genre { get; set; }
    public int? Year { get; set; }
}

public class ChorusEncoreSearchFilter
{
    public bool Exact { get; set; }
    public bool Exclude { get; set; }
    public string? Value { get; set; } = string.Empty;
}
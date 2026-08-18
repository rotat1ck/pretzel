namespace Pretzel.Core.Enums;

public enum InstrumentType
{
    Guitar,
    Bass,
    Keys,
    Drums,
    Vocals
}

public static class InstrumentTypeResolver
{
    public static InstrumentType? ResolveFromName(string instrumentName)
    {
        if (string.IsNullOrWhiteSpace(instrumentName))
        {
            return default;
        }

        if (Enum.TryParse<InstrumentType>(instrumentName, ignoreCase: true, out var result) && Enum.IsDefined(result))
        {
            return result;
        }

        return default;
    }
}
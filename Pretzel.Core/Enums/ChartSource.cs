namespace Pretzel.Core.Enums;

public enum ChartSource
{
    ChorusEncore,
    RhythmVerse
}

public static class ChartSourceResolver
{
    private static Dictionary<ChartSource, string> sourceUriMap = new()
    {
        [ChartSource.ChorusEncore] = "https://api.enchor.us",
        [ChartSource.RhythmVerse] = "https://rhythmverse.co",
    };

    public static Uri ResolveBaseUri(this ChartSource source)
    {
        if (!sourceUriMap.TryGetValue(source, out var uri))
        {
            throw new KeyNotFoundException($"Base uri for {source} was not defined");
        }

        return new Uri(uri);
    }
}
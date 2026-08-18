namespace Pretzel.Core.Enums;

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard,
    Expert
}

public static class DifficultyLevelResolver
{
    private static readonly Dictionary<string, DifficultyLevel> ShortNameMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["e"] = DifficultyLevel.Easy,
        ["m"] = DifficultyLevel.Medium,
        ["h"] = DifficultyLevel.Hard,
        ["x"] = DifficultyLevel.Expert
    };

    public static DifficultyLevel? ResolveFromName(string difficultyLevel)
    {
        if (string.IsNullOrWhiteSpace(difficultyLevel))
        {
            return default;
        }

        if (difficultyLevel.Length == 1 && ShortNameMap.TryGetValue(difficultyLevel, out var result))
        {
            return result;
        }

        if (Enum.TryParse<DifficultyLevel>(difficultyLevel, ignoreCase: true, out result) && Enum.IsDefined(result))
        {
            return result;
        }

        return default;
    }
}
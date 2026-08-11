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
    public static DifficultyLevel? ResolveFromName(string difficultyLevel)
    {
        if (string.IsNullOrWhiteSpace(difficultyLevel))
        {
            return default;
        }

        if (Enum.TryParse<DifficultyLevel>(difficultyLevel, ignoreCase: true, out var result))
        {
            return result;
        }

        return default;
    }
}
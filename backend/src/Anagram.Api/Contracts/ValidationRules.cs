namespace Anagram.Api.Contracts;

internal static class ValidationRules
{
    public const int MaxWordLength = 50;
    public const int MaxTextLength = 1_000;

    // Letters (including accented Latin-1 letters such as é) and apostrophes,
    // with optional surrounding whitespace which is trimmed later.
    public const string WordPattern = @"^\s*[A-Za-zÀ-ÿ']+\s*$";
    public const string WordPatternMessage = "A word may only contain letters and apostrophes.";
}

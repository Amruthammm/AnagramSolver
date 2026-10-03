namespace Anagram.Domain;

/// <summary>
/// Single place that defines how raw user / file text is normalised,
/// so the dictionary and user input are always compared like-for-like.
/// The exercise is case-insensitive, so everything is lower-cased.
/// </summary>
public static class WordText
{
    public static string Normalise(string? value) =>
        (value ?? string.Empty).Trim().ToLowerInvariant();
}

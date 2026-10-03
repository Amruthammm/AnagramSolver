namespace Anagram.Domain;

/// <summary>
/// Two strings are anagrams of each other exactly when they contain the same
/// characters the same number of times. Sorting the characters gives a
/// canonical "signature" for that: "dogs" -> "dgos", "gods" -> "dgos".
///
/// The signature is stored alongside every dictionary word (and indexed), so
/// finding anagrams becomes a single equality lookup instead of comparing the
/// input against ~80,000 words.
/// </summary>
public static class AnagramSignature
{
    public static string For(string value) =>
        new(WordText.Normalise(value).OrderBy(c => c).ToArray());

    public static bool AreAnagrams(string first, string second) =>
        For(first) == For(second);
}

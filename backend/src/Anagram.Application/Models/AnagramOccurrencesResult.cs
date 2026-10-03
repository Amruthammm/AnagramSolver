namespace Anagram.Application.Models;

/// <summary>A single match: where it starts in the text and the letters found there.</summary>
public sealed record AnagramOccurrence(int Index, string Value);

public sealed record AnagramOccurrencesResult(string Text, string Word, IReadOnlyList<AnagramOccurrence> Occurrences)
{
    public int Count => Occurrences.Count;
}

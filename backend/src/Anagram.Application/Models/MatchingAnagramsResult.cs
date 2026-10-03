namespace Anagram.Application.Models;

public sealed record MatchingAnagramsResult(string Input, IReadOnlyList<string> Anagrams)
{
    public int Count => Anagrams.Count;
}

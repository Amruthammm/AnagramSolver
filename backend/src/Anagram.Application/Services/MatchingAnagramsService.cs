using Anagram.Application.Abstractions;
using Anagram.Application.Models;
using Anagram.Domain;

namespace Anagram.Application.Services;

public sealed class MatchingAnagramsService : IMatchingAnagramsService
{
    private readonly IWordRepository _words;

    public MatchingAnagramsService(IWordRepository words)
    {
        _words = words;
    }

    public async Task<MatchingAnagramsResult> FindAsync(string input, CancellationToken cancellationToken = default)
    {
        var word = WordText.Normalise(input);

        if (word.Length == 0)
        {
            return new MatchingAnagramsResult(word, Array.Empty<string>());
        }

        // Every anagram shares the same sorted-letter signature, and a different
        // length always gives a different signature, so "dogs" can never match
        // "dog" or "god" - the "must use all letters" rule comes for free.
        var candidates = await _words.GetWordsBySignatureAsync(AnagramSignature.For(word), cancellationToken);

        var anagrams = candidates
            .Where(candidate => candidate != word) // a word is not an anagram of itself
            .Distinct()
            .OrderBy(candidate => candidate)
            .ToList();

        return new MatchingAnagramsResult(word, anagrams);
    }
}

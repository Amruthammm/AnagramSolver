using Anagram.Application.Abstractions;
using Anagram.Application.Models;
using Anagram.Domain;

namespace Anagram.Application.Services;

/// <summary>
/// Slides a window the length of <c>word</c> across <c>text</c>, left to right,
/// and keeps every window whose signature matches the word's signature.
///
/// Example: text "thegodsanddogsweredogged", word "dog"
///   the[god]sand[dog]swere[dog]ged  ->  3
///
/// Windows overlap, so every starting position is checked. This is
/// O(n * k log k) for a text of length n and word of length k, which is
/// perfectly fine for form input. For very large texts a rolling
/// letter-frequency count would make it O(n).
/// </summary>
public sealed class AnagramOccurrenceService : IAnagramOccurrenceService
{
    public AnagramOccurrencesResult FindOccurrences(string text, string word)
    {
        var normalisedText = WordText.Normalise(text);
        var normalisedWord = WordText.Normalise(word);

        if (normalisedWord.Length == 0 || normalisedWord.Length > normalisedText.Length)
        {
            return new AnagramOccurrencesResult(normalisedText, normalisedWord, Array.Empty<AnagramOccurrence>());
        }

        var target = AnagramSignature.For(normalisedWord);
        var windowCount = normalisedText.Length - normalisedWord.Length + 1;

        var occurrences = Enumerable.Range(0, windowCount)
            .Select(index => new AnagramOccurrence(index, normalisedText.Substring(index, normalisedWord.Length)))
            .Where(window => AnagramSignature.For(window.Value) == target)
            .ToList();

        return new AnagramOccurrencesResult(normalisedText, normalisedWord, occurrences);
    }
}

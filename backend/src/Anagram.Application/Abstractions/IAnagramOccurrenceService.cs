using Anagram.Application.Models;

namespace Anagram.Application.Abstractions;

/// <summary>Feature 2: count how often a word, or any anagram of it, appears inside a text.</summary>
public interface IAnagramOccurrenceService
{
    AnagramOccurrencesResult FindOccurrences(string text, string word);
}

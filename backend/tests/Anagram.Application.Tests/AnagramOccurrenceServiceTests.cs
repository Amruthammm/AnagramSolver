using Anagram.Application.Services;

namespace Anagram.Application.Tests;

public class AnagramOccurrenceServiceTests
{
    private readonly AnagramOccurrenceService _sut = new();

    [Fact]
    public void Spec_example_counts_three_occurrences_left_to_right()
    {
        var result = _sut.FindOccurrences("thegodsanddogsweredogged", "dog");

        Assert.Equal(3, result.Count);
        Assert.Equal(new[] { "god", "dog", "dog" }, result.Occurrences.Select(o => o.Value));
        Assert.Equal(new[] { 3, 10, 18 }, result.Occurrences.Select(o => o.Index));
    }

    [Theory]
    [InlineData("TheGodsAndDogs", "DOG", 2)] // case-insensitive
    [InlineData("abab", "ab", 3)]            // overlapping windows: ab, ba, ab
    [InlineData("dog", "dogs", 0)]           // word longer than text
    [InlineData("cat", "dog", 0)]            // no match
    [InlineData("dog", "  ", 0)]             // blank word
    public void Counts_anagram_windows(string text, string word, int expected)
    {
        Assert.Equal(expected, _sut.FindOccurrences(text, word).Count);
    }
}

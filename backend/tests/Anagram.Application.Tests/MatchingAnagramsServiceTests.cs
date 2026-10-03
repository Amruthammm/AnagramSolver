using Anagram.Application.Abstractions;
using Anagram.Application.Services;
using Anagram.Domain;

namespace Anagram.Application.Tests;

public class MatchingAnagramsServiceTests
{
    private static readonly string[] Dictionary = { "dog", "god", "dogs", "gods", "cat", "act" };

    private readonly MatchingAnagramsService _sut = new(new InMemoryWordRepository(Dictionary));

    [Fact]
    public async Task Spec_example_dogs_returns_gods_but_not_shorter_words()
    {
        var result = await _sut.FindAsync("dogs");

        Assert.Equal(new[] { "gods" }, result.Anagrams);
    }

    [Fact]
    public async Task Input_is_case_insensitive_and_trimmed()
    {
        var result = await _sut.FindAsync("  CAT ");

        Assert.Equal("cat", result.Input);
        Assert.Equal(new[] { "act" }, result.Anagrams);
    }

    [Fact]
    public async Task Returns_empty_list_when_nothing_matches()
    {
        var result = await _sut.FindAsync("xyz");

        Assert.Empty(result.Anagrams);
    }

    /// <summary>Test double that mimics the indexed signature lookup the database performs.</summary>
    private sealed class InMemoryWordRepository : IWordRepository
    {
        private readonly ILookup<string, string> _bySignature;

        public InMemoryWordRepository(IEnumerable<string> words) =>
            _bySignature = words.ToLookup(AnagramSignature.For);

        public Task<IReadOnlyList<string>> GetWordsBySignatureAsync(string signature, CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<string>>(_bySignature[signature].ToList());
    }
}

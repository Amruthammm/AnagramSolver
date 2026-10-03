using Anagram.Application.Models;

namespace Anagram.Application.Abstractions;

/// <summary>Feature 1: list every dictionary word that is an anagram of the input.</summary>
public interface IMatchingAnagramsService
{
    Task<MatchingAnagramsResult> FindAsync(string input, CancellationToken cancellationToken = default);
}

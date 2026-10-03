using System.ComponentModel.DataAnnotations;

namespace Anagram.Api.Contracts;

public sealed class MatchingAnagramsRequest
{
    /// <summary>The word to find anagrams for, e.g. "dogs".</summary>
    [Required]
    [StringLength(ValidationRules.MaxWordLength)]
    [RegularExpression(ValidationRules.WordPattern, ErrorMessage = ValidationRules.WordPatternMessage)]
    public string Word { get; init; } = string.Empty;
}

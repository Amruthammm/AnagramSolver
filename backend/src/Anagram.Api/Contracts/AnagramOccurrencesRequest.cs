using System.ComponentModel.DataAnnotations;

namespace Anagram.Api.Contracts;

public sealed class AnagramOccurrencesRequest
{
    /// <summary>The text to search, e.g. "thegodsanddogsweredogged".</summary>
    [Required]
    [StringLength(ValidationRules.MaxTextLength)]
    public string Text { get; init; } = string.Empty;

    /// <summary>The word whose anagrams are counted, e.g. "dog".</summary>
    [Required]
    [StringLength(ValidationRules.MaxWordLength)]
    [RegularExpression(ValidationRules.WordPattern, ErrorMessage = ValidationRules.WordPatternMessage)]
    public string Word { get; init; } = string.Empty;
}

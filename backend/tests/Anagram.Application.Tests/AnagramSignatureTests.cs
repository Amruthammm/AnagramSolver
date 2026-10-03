using Anagram.Domain;

namespace Anagram.Application.Tests;

public class AnagramSignatureTests
{
    [Theory]
    [InlineData("dogs", "dgos")]
    [InlineData("GODS", "dgos")]
    [InlineData(" god ", "dgo")]
    public void Signature_is_sorted_lowercase_letters(string input, string expected)
    {
        Assert.Equal(expected, AnagramSignature.For(input));
    }

    [Theory]
    [InlineData("listen", "silent", true)]
    [InlineData("dogs", "dog", false)]
    public void AreAnagrams_compares_signatures(string first, string second, bool expected)
    {
        Assert.Equal(expected, AnagramSignature.AreAnagrams(first, second));
    }
}

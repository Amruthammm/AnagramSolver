namespace Anagram.Domain;

/// <summary>
/// A dictionary word. The <see cref="Signature"/> is derived from the text
/// when the word is created, so the two can never get out of step.
/// </summary>
public class Word
{
    // Required by EF Core for materialisation.
    private Word()
    {
    }

    private Word(string text)
    {
        Text = text;
        Signature = AnagramSignature.For(text);
    }

    public int Id { get; private set; }

    public string Text { get; private set; } = string.Empty;

    /// <summary>The word's characters in sorted order, e.g. "gods" -> "dgos".</summary>
    public string Signature { get; private set; } = string.Empty;

    public static Word Create(string text)
    {
        var normalised = WordText.Normalise(text);

        if (normalised.Length == 0)
        {
            throw new ArgumentException("A word must contain at least one character.", nameof(text));
        }

        return new Word(normalised);
    }
}

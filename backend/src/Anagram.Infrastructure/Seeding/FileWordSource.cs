using System.Text;
using Anagram.Domain;

namespace Anagram.Infrastructure.Seeding;

/// <summary>
/// Reads brit-a-z.txt: one word per line, CRLF line endings, Latin-1 encoded
/// (so words such as "cliché" and "déjà" are read correctly rather than as
/// replacement characters).
/// </summary>
public sealed class FileWordSource : IWordSource
{
    private readonly string _filePath;

    public FileWordSource(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<string> ReadWords()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"Dictionary file not found at '{_filePath}'.", _filePath);
        }

        return File.ReadLines(_filePath, Encoding.Latin1)
            .Select(WordText.Normalise)
            .Where(word => word.Length > 0)
            .Distinct();
    }
}

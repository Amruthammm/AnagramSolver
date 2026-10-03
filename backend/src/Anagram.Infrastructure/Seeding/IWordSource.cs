namespace Anagram.Infrastructure.Seeding;

/// <summary>Where the raw dictionary words come from (a file today; could be anything).</summary>
public interface IWordSource
{
    IEnumerable<string> ReadWords();
}

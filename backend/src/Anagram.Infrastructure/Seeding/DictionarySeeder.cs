using Anagram.Domain;
using Anagram.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Anagram.Infrastructure.Seeding;

/// <summary>
/// Populates the Words table from the word source the first time the app runs.
/// Inserts in batches inside one transaction so ~80k rows load quickly and the
/// table is never left half-populated.
/// </summary>
public sealed class DictionarySeeder
{
    private const int BatchSize = 5_000;

    private readonly AnagramDbContext _db;
    private readonly IWordSource _source;
    private readonly ILogger<DictionarySeeder> _logger;

    public DictionarySeeder(AnagramDbContext db, IWordSource source, ILogger<DictionarySeeder> logger)
    {
        _db = db;
        _source = source;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.Words.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Dictionary already loaded; skipping seed.");
            return;
        }

        _logger.LogInformation("Loading dictionary words into the database...");

        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        var total = 0;
        foreach (var batch in _source.ReadWords().Chunk(BatchSize))
        {
            _db.Words.AddRange(batch.Select(Word.Create));
            await _db.SaveChangesAsync(cancellationToken);
            _db.ChangeTracker.Clear(); // keep memory flat between batches
            total += batch.Length;
        }

        await transaction.CommitAsync(cancellationToken);

        _logger.LogInformation("Loaded {Count} dictionary words.", total);
    }
}

using Anagram.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Anagram.Infrastructure.Persistence;

public sealed class WordRepository : IWordRepository
{
    private readonly AnagramDbContext _db;

    public WordRepository(AnagramDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<string>> GetWordsBySignatureAsync(string signature, CancellationToken cancellationToken = default) =>
        await _db.Words
            .AsNoTracking()
            .Where(word => word.Signature == signature)
            .Select(word => word.Text)
            .ToListAsync(cancellationToken);
}

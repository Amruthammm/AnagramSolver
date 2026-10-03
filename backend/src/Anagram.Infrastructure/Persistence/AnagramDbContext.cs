using Anagram.Domain;
using Microsoft.EntityFrameworkCore;

namespace Anagram.Infrastructure.Persistence;

public sealed class AnagramDbContext : DbContext
{
    public AnagramDbContext(DbContextOptions<AnagramDbContext> options)
        : base(options)
    {
    }

    public DbSet<Word> Words => Set<Word>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnagramDbContext).Assembly);
    }
}

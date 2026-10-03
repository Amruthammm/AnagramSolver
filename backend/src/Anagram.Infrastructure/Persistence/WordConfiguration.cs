using Anagram.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anagram.Infrastructure.Persistence;

/// <summary>
/// Table design:
///   Words(Id, Text, Signature)
///   - Text is unique (one row per dictionary word).
///   - Signature is indexed and the index INCLUDEs Text (SQL Server), so the
///     anagram lookup is a single index seek with no extra key lookups.
/// </summary>
public sealed class WordConfiguration : IEntityTypeConfiguration<Word>
{
    private const int MaxWordLength = 64;

    public void Configure(EntityTypeBuilder<Word> builder)
    {
        builder.ToTable("Words");

        builder.HasKey(word => word.Id);

        builder.Property(word => word.Text)
            .HasMaxLength(MaxWordLength)
            .IsRequired();

        builder.Property(word => word.Signature)
            .HasMaxLength(MaxWordLength)
            .IsRequired();

        builder.HasIndex(word => word.Text)
            .IsUnique();

        builder.HasIndex(word => word.Signature)
            .IncludeProperties(word => word.Text);
    }
}

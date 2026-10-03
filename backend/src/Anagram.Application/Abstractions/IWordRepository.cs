namespace Anagram.Application.Abstractions;

/// <summary>
/// Read access to the dictionary. The application layer depends on this
/// abstraction only; the EF Core / SQL Server implementation lives in
/// Infrastructure (Dependency Inversion).
/// </summary>
public interface IWordRepository
{
    Task<IReadOnlyList<string>> GetWordsBySignatureAsync(string signature, CancellationToken cancellationToken = default);
}

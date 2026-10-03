namespace Anagram.Infrastructure;

public enum DatabaseProvider
{
    SqlServer,
    Sqlite
}

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public DatabaseProvider Provider { get; set; } = DatabaseProvider.SqlServer;

    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>Path to brit-a-z.txt, relative to the app's output folder or absolute.</summary>
    public string DictionaryFilePath { get; set; } = "Data/brit-a-z.txt";
}

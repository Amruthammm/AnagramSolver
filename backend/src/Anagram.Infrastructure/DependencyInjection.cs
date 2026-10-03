using Anagram.Application.Abstractions;
using Anagram.Infrastructure.Persistence;
using Anagram.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Anagram.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, DatabaseOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            throw new InvalidOperationException($"'{DatabaseOptions.SectionName}:ConnectionString' is not configured.");
        }

        services.AddDbContext<AnagramDbContext>(db =>
        {
            switch (options.Provider)
            {
                case DatabaseProvider.Sqlite:
                    db.UseSqlite(options.ConnectionString);
                    break;
                default:
                    db.UseSqlServer(options.ConnectionString);
                    break;
            }
        });

        services.AddScoped<IWordRepository, WordRepository>();

        var dictionaryPath = Path.IsPathRooted(options.DictionaryFilePath)
            ? options.DictionaryFilePath
            : Path.Combine(AppContext.BaseDirectory, options.DictionaryFilePath);

        services.AddSingleton<IWordSource>(new FileWordSource(dictionaryPath));
        services.AddScoped<DictionarySeeder>();

        return services;
    }

    /// <summary>Creates the database if needed and loads the dictionary on first run.</summary>
    public static async Task InitialiseDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();

        var db = scope.ServiceProvider.GetRequiredService<AnagramDbContext>();
        await db.Database.EnsureCreatedAsync(cancellationToken);

        var seeder = scope.ServiceProvider.GetRequiredService<DictionarySeeder>();
        await seeder.SeedAsync(cancellationToken);
    }
}

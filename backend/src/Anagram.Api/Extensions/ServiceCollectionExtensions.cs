using Anagram.Application.Abstractions;
using Anagram.Application.Services;

namespace Anagram.Api.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>Registers the application-layer services (the use cases).</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMatchingAnagramsService, MatchingAnagramsService>();
        services.AddSingleton<IAnagramOccurrenceService, AnagramOccurrenceService>(); // stateless

        return services;
    }

    public static IServiceCollection AddClientCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        services.AddCors(cors => cors.AddPolicy(CorsPolicyName, policy =>
            policy.WithOrigins(origins).AllowAnyHeader().WithMethods("GET")));

        return services;
    }

    public const string CorsPolicyName = "AngularClient";
}

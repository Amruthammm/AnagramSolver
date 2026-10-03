using Anagram.Api.Extensions;
using Anagram.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var databaseOptions = builder.Configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>()
    ?? throw new InvalidOperationException($"Missing '{DatabaseOptions.SectionName}' configuration section.");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddClientCors(builder.Configuration);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(databaseOptions);

var app = builder.Build();

// Create the database and load brit-a-z.txt on first run.
await app.Services.InitialiseDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseCors(ServiceCollectionExtensions.CorsPolicyName);
app.MapControllers();

app.Run();

// Exposed so integration tests can use WebApplicationFactory<Program>.
public partial class Program { }

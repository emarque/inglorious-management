using Microsoft.EntityFrameworkCore;
using SoftballManager.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Configure the MySQL connection using ConnectionStrings__DefaultConnection.");
}

builder.Services.AddDbContext<SoftballDbContext>(options => options.UseMySQL(connectionString));

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("Liveness");

app.MapGet("/health/ready", async (SoftballDbContext db, CancellationToken cancellationToken) =>
    await db.Database.CanConnectAsync(cancellationToken)
        ? Results.Ok(new { status = "ready" })
        : Results.StatusCode(StatusCodes.Status503ServiceUnavailable))
    .WithName("Readiness");

var api = app.MapGroup("/api");

api.MapGet("/seasons", async (SoftballDbContext db, CancellationToken cancellationToken) =>
    await db.Seasons.AsNoTracking().OrderBy(season => season.StartsOn).ToListAsync(cancellationToken));

api.MapGet("/teams", async (SoftballDbContext db, CancellationToken cancellationToken) =>
    await db.Teams.AsNoTracking().OrderBy(team => team.Name).ToListAsync(cancellationToken));

api.MapGet("/players", async (SoftballDbContext db, CancellationToken cancellationToken) =>
    await db.Players.AsNoTracking().OrderBy(player => player.LastName).ThenBy(player => player.FirstName)
        .ToListAsync(cancellationToken));

api.MapGet("/games", async (SoftballDbContext db, CancellationToken cancellationToken) =>
    await db.Games.AsNoTracking().OrderBy(game => game.StartsAtUtc).ToListAsync(cancellationToken));

await app.RunAsync();

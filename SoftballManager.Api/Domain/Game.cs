namespace SoftballManager.Api.Domain;

public sealed class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SeasonId { get; set; }
    public Guid HomeTeamId { get; set; }
    public Guid? AwayTeamId { get; set; }
    public string? OpponentName { get; set; }
    public DateTimeOffset StartsAtUtc { get; set; }
    public string? Location { get; set; }
}

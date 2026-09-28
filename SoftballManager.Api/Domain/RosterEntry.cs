namespace SoftballManager.Api.Domain;

public sealed class RosterEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SeasonId { get; set; }
    public Guid TeamId { get; set; }
    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    public string Role { get; set; } = string.Empty;
}

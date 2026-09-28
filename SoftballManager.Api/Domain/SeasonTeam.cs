namespace SoftballManager.Api.Domain;

public sealed class SeasonTeam
{
    public Guid SeasonId { get; set; }
    public Season Season { get; set; } = null!;
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;
}

namespace SoftballManager.Api.Domain;

public sealed class Team
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
}

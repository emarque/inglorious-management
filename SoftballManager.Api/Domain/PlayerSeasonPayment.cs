namespace SoftballManager.Api.Domain;

public sealed class PlayerSeasonPayment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    public Guid SeasonId { get; set; }
    public Season Season { get; set; } = null!;
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Due;
    public DateTimeOffset? PaidAtUtc { get; set; }
}

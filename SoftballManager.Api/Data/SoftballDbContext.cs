using Microsoft.EntityFrameworkCore;
using SoftballManager.Api.Domain;

namespace SoftballManager.Api.Data;

public sealed class SoftballDbContext(DbContextOptions<SoftballDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<PlayerSeasonPayment> PlayerSeasonPayments => Set<PlayerSeasonPayment>();
    public DbSet<RosterEntry> RosterEntries => Set<RosterEntry>();
    public DbSet<Season> Seasons => Set<Season>();
    public DbSet<SeasonTeam> SeasonTeams => Set<SeasonTeam>();
    public DbSet<Team> Teams => Set<Team>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Season>(entity =>
        {
            entity.ToTable("seasons");
            entity.HasKey(season => season.Id);
            entity.Property(season => season.Name).HasMaxLength(100).IsRequired();
            entity.HasIndex(season => season.Name).IsUnique();
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("teams");
            entity.HasKey(team => team.Id);
            entity.Property(team => team.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("players");
            entity.HasKey(player => player.Id);
            entity.Property(player => player.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(player => player.LastName).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<SeasonTeam>(entity =>
        {
            entity.ToTable("season_teams");
            entity.HasKey(seasonTeam => new { seasonTeam.SeasonId, seasonTeam.TeamId });
            entity.HasOne(seasonTeam => seasonTeam.Season)
                .WithMany()
                .HasForeignKey(seasonTeam => seasonTeam.SeasonId);
            entity.HasOne(seasonTeam => seasonTeam.Team)
                .WithMany()
                .HasForeignKey(seasonTeam => seasonTeam.TeamId);
        });

        modelBuilder.Entity<RosterEntry>(entity =>
        {
            entity.ToTable("roster_entries");
            entity.HasKey(entry => entry.Id);
            entity.Property(entry => entry.Role).HasMaxLength(50).IsRequired();
            entity.HasIndex(entry => new { entry.SeasonId, entry.TeamId, entry.PlayerId }).IsUnique();
            entity.HasOne<SeasonTeam>()
                .WithMany()
                .HasForeignKey(entry => new { entry.SeasonId, entry.TeamId });
            entity.HasOne(entry => entry.Player)
                .WithMany()
                .HasForeignKey(entry => entry.PlayerId);
        });

        modelBuilder.Entity<PlayerSeasonPayment>(entity =>
        {
            entity.ToTable("player_season_payments");
            entity.HasKey(payment => payment.Id);
            entity.Property(payment => payment.Amount).HasPrecision(10, 2);
            entity.Property(payment => payment.Status).HasConversion<string>().HasMaxLength(20);
            entity.HasIndex(payment => new { payment.PlayerId, payment.SeasonId }).IsUnique();
            entity.HasOne(payment => payment.Player)
                .WithMany()
                .HasForeignKey(payment => payment.PlayerId);
            entity.HasOne(payment => payment.Season)
                .WithMany()
                .HasForeignKey(payment => payment.SeasonId);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("games", table => table.HasCheckConstraint(
                "ck_games_opponent",
                "((AwayTeamId IS NOT NULL AND OpponentName IS NULL) OR " +
                "(AwayTeamId IS NULL AND OpponentName IS NOT NULL))"));
            entity.HasKey(game => game.Id);
            entity.Property(game => game.OpponentName).HasMaxLength(100);
            entity.Property(game => game.Location).HasMaxLength(200);
            entity.HasOne<SeasonTeam>()
                .WithMany()
                .HasForeignKey(game => new { game.SeasonId, game.HomeTeamId })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<SeasonTeam>()
                .WithMany()
                .HasForeignKey(game => new { game.SeasonId, game.AwayTeamId })
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

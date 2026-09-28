using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftballManager.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    StartsOn = table.Column<DateOnly>(type: "date", nullable: false),
                    EndsOn = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seasons", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_season_payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    PlayerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SeasonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    PaidAtUtc = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_season_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_player_season_payments_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_player_season_payments_seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "season_teams",
                columns: table => new
                {
                    SeasonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_season_teams", x => new { x.SeasonId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_season_teams_seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_season_teams_teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    SeasonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    HomeTeamId = table.Column<Guid>(type: "char(36)", nullable: false),
                    AwayTeamId = table.Column<Guid>(type: "char(36)", nullable: true),
                    OpponentName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StartsAtUtc = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    Location = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.Id);
                    table.CheckConstraint("ck_games_opponent", "((AwayTeamId IS NOT NULL AND OpponentName IS NULL) OR (AwayTeamId IS NULL AND OpponentName IS NOT NULL))");
                    table.ForeignKey(
                        name: "FK_games_season_teams_SeasonId_AwayTeamId",
                        columns: x => new { x.SeasonId, x.AwayTeamId },
                        principalTable: "season_teams",
                        principalColumns: new[] { "SeasonId", "TeamId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_games_season_teams_SeasonId_HomeTeamId",
                        columns: x => new { x.SeasonId, x.HomeTeamId },
                        principalTable: "season_teams",
                        principalColumns: new[] { "SeasonId", "TeamId" },
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roster_entries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    SeasonId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PlayerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roster_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_roster_entries_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roster_entries_season_teams_SeasonId_TeamId",
                        columns: x => new { x.SeasonId, x.TeamId },
                        principalTable: "season_teams",
                        principalColumns: new[] { "SeasonId", "TeamId" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_games_SeasonId_AwayTeamId",
                table: "games",
                columns: new[] { "SeasonId", "AwayTeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_games_SeasonId_HomeTeamId",
                table: "games",
                columns: new[] { "SeasonId", "HomeTeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_player_season_payments_PlayerId_SeasonId",
                table: "player_season_payments",
                columns: new[] { "PlayerId", "SeasonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_season_payments_SeasonId",
                table: "player_season_payments",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_roster_entries_PlayerId",
                table: "roster_entries",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_roster_entries_SeasonId_TeamId_PlayerId",
                table: "roster_entries",
                columns: new[] { "SeasonId", "TeamId", "PlayerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_season_teams_TeamId",
                table: "season_teams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_seasons_Name",
                table: "seasons",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "player_season_payments");

            migrationBuilder.DropTable(
                name: "roster_entries");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "season_teams");

            migrationBuilder.DropTable(
                name: "seasons");

            migrationBuilder.DropTable(
                name: "teams");
        }
    }
}

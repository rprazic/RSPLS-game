using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    PlayerChoice = table.Column<int>(type: "integer", nullable: false),
                    ComputerChoice = table.Column<int>(type: "integer", nullable: false),
                    Result = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    PlayerChoiceName = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ComputerChoiceName = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameResults", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameResults_PlayedAt",
                table: "GameResults",
                column: "PlayedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameResults");
        }
    }
}

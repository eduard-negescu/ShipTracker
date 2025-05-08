using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShipTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddVoyageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Voyages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    VoyageDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VoyageStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VoyageEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeparturePortId = table.Column<int>(type: "integer", nullable: false),
                    ArrivalPortId = table.Column<int>(type: "integer", nullable: false),
                    ShipId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voyages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voyages_Ports_ArrivalPortId",
                        column: x => x.ArrivalPortId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Voyages_Ports_DeparturePortId",
                        column: x => x.DeparturePortId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Voyages_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voyages_ArrivalPortId",
                table: "Voyages",
                column: "ArrivalPortId");

            migrationBuilder.CreateIndex(
                name: "IX_Voyages_DeparturePortId",
                table: "Voyages",
                column: "DeparturePortId");

            migrationBuilder.CreateIndex(
                name: "IX_Voyages_ShipId",
                table: "Voyages",
                column: "ShipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voyages");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpAn.App.MorphTrade.Domain.Flows.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:timescaledb", ",,");

            migrationBuilder.CreateTable(
                name: "Flows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StatusDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CallResponseEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FlowId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsBacktesting = table.Column<bool>(type: "boolean", nullable: false),
                    TradeCall = table.Column<string>(type: "text", nullable: false),
                    Ticker_Symbol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ticker_Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Ticker_Volume = table.Column<decimal>(type: "numeric", nullable: true),
                    Ticker_Quantity = table.Column<long>(type: "bigint", nullable: false),
                    Ticker_Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallResponseEvents", x => new { x.Id, x.Timestamp });
                    table.ForeignKey(
                        name: "FK_CallResponseEvents_Flows_FlowId",
                        column: x => x.FlowId,
                        principalTable: "Flows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallResponseEvents_FlowId",
                table: "CallResponseEvents",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_Flows_Id",
                table: "Flows",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallResponseEvents");

            migrationBuilder.DropTable(
                name: "Flows");
        }
    }
}

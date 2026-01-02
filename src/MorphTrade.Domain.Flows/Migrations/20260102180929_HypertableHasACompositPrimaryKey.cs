using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpAn.App.MorphTrade.Domain.Flows.Migrations
{
    /// <inheritdoc />
    public partial class HypertableHasACompositPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CallResponseEvents",
                table: "CallResponseEvents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CallResponseEvents",
                table: "CallResponseEvents",
                columns: new[] { "Id", "Timestamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CallResponseEvents",
                table: "CallResponseEvents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CallResponseEvents",
                table: "CallResponseEvents",
                column: "Id");
        }
    }
}

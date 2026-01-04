using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpAn.App.MorphTrade.Domain.Flows.Migrations
{
    /// <inheritdoc />
    public partial class MadeFlowDomainEntitiesAuthorizedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Flows",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CallResponseEvents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Flows");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CallResponseEvents");
        }
    }
}

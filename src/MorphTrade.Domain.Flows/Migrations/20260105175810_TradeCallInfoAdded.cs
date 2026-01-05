using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpAn.App.MorphTrade.Domain.Flows.Migrations
{
    /// <inheritdoc />
    public partial class TradeCallInfoAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TradeCallInfo_Entry",
                table: "CallResponseEvents",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TradeCallInfo_StopLoss",
                table: "CallResponseEvents",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TradeCallInfo_TakeProfit",
                table: "CallResponseEvents",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TradeCallInfo_TradeCall",
                table: "CallResponseEvents",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TradeCallInfo_Entry",
                table: "CallResponseEvents");

            migrationBuilder.DropColumn(
                name: "TradeCallInfo_StopLoss",
                table: "CallResponseEvents");

            migrationBuilder.DropColumn(
                name: "TradeCallInfo_TakeProfit",
                table: "CallResponseEvents");

            migrationBuilder.DropColumn(
                name: "TradeCallInfo_TradeCall",
                table: "CallResponseEvents");
        }
    }
}

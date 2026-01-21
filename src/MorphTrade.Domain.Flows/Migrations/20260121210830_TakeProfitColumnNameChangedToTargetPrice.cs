using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpAn.App.MorphTrade.Domain.Flows.Migrations
{
    /// <inheritdoc />
    public partial class TakeProfitColumnNameChangedToTargetPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TradeCallInfo_TakeProfit",
                table: "CallResponseEvents",
                newName: "TradeCallInfo_TargetPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TradeCallInfo_TargetPrice",
                table: "CallResponseEvents",
                newName: "TradeCallInfo_TakeProfit");
        }
    }
}

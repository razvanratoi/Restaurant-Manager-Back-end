using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManager.Migrations
{
    /// <inheritdoc />
    public partial class ceasdsafsaajsfvasf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_WaiterId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WaiterId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_WaiterId",
                table: "Orders",
                column: "WaiterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_WaiterId",
                table: "Orders",
                column: "WaiterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

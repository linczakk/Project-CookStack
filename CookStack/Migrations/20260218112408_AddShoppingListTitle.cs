using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookStackShared.Migrations
{
    /// <inheritdoc />
    public partial class AddShoppingListTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ShoppingLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "ShoppingLists");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookStack.Shared.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeLastVisitedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastVisitedAt",
                table: "Recipes",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastVisitedAt",
                table: "Recipes");
        }
    }
}

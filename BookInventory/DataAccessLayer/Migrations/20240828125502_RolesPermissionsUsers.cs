using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookInventory.Migrations
{
    /// <inheritdoc />
    public partial class RolesPermissionsUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetPasswordExpires",
                table: "Users",
                newName: "ResetTokenExpires");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetTokenExpires",
                table: "Users",
                newName: "ResetPasswordExpires");
        }
    }
}

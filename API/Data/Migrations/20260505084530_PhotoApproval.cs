using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class PhotoApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Members",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "AspNetUsers",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Photos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Members",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "AspNetUsers",
                newName: "Url");
        }
    }
}

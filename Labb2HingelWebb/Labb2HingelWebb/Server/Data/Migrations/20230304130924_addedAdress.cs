using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb2HingelWebb.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedAdress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    public partial class Initial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_RefreshTokens_Admins_AdminId",
                table: "Admins_RefreshTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_RefreshTokens_Admins_AdminId",
                table: "Admins_RefreshTokens",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_RefreshTokens_Admins_AdminId",
                table: "Admins_RefreshTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_RefreshTokens_Admins_AdminId",
                table: "Admins_RefreshTokens",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

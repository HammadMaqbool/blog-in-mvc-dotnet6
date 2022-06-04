using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogTutorial.Migrations
{
    public partial class updateProfilewithPass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Tbl_Profile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Tbl_Profile");
        }
    }
}

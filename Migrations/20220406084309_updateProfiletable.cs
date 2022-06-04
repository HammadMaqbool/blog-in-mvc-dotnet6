using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogTutorial.Migrations
{
    public partial class updateProfiletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "Tbl_Profile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "username",
                table: "Tbl_Profile");
        }
    }
}

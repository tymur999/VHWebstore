using Microsoft.EntityFrameworkCore.Migrations;

namespace VHacksWebstore.Core.App.Migrations.WebstoreDb
{
    public partial class New_Features : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ListString",
                table: "Products",
                newName: "Images");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Images",
                table: "Products",
                newName: "ListString");
        }
    }
}

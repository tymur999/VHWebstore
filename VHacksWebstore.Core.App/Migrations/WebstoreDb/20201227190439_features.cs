using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VHacksWebstore.Core.App.Migrations.WebstoreDb
{
    public partial class features : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "TopBroughtProducts");

            migrationBuilder.DropTable(
                name: "TopRatedProducts");

            migrationBuilder.RenameColumn(
                name: "ShortDesc",
                table: "Ratings",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "LongDesc",
                table: "Ratings",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "ListString",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PrimaryImage",
                table: "Products",
                type: "BLOB",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListString",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PrimaryImage",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Ratings",
                newName: "ShortDesc");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Ratings",
                newName: "LongDesc");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProductRatingId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Image_Ratings_ProductRatingId",
                        column: x => x.ProductRatingId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopBroughtProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopBroughtProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopBroughtProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TopRatedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopRatedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopRatedProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Image_ProductId",
                table: "Image",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_ProductRatingId",
                table: "Image",
                column: "ProductRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_TopBroughtProducts_ProductId",
                table: "TopBroughtProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TopRatedProducts_ProductId",
                table: "TopRatedProducts",
                column: "ProductId");
        }
    }
}

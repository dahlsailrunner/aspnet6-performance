using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarvedRock.Data.Migrations
{
    public partial class Ratings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Products",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AggregateRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    NumberOfRatings = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRatings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_RatingId",
                table: "Products",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductRatings_RatingId",
                table: "Products",
                column: "RatingId",
                principalTable: "ProductRatings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductRatings_RatingId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductRatings");

            migrationBuilder.DropIndex(
                name: "IX_Products_RatingId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Products");
        }
    }
}

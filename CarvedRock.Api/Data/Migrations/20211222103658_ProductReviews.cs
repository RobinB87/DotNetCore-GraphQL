using Microsoft.EntityFrameworkCore.Migrations;

namespace CarvedRock.Api.Data.Migrations
{
    public partial class ProductReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ProductReviews",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(int),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Review",
                table: "ProductReviews",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Title",
                table: "ProductReviews",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "Review",
                table: "ProductReviews",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

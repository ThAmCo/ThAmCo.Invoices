using Microsoft.EntityFrameworkCore.Migrations;

namespace Invoices.Data.Migrations
{
    public partial class RemoveProfilesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Profiles_ProfileId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ProfileId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Invoices",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Invoices");

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ProfileId",
                table: "Invoices",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Profiles_ProfileId",
                table: "Invoices",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

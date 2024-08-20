using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PestControll_CRM.Migrations
{
    /// <inheritdoc />
    public partial class Clients2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalPersons_TaxSystems_taxsystem_id",
                table: "LegalPersons");

            migrationBuilder.AlterColumn<int>(
                name: "taxsystem_id",
                table: "LegalPersons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LegalPersons_TaxSystems_taxsystem_id",
                table: "LegalPersons",
                column: "taxsystem_id",
                principalTable: "TaxSystems",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalPersons_TaxSystems_taxsystem_id",
                table: "LegalPersons");

            migrationBuilder.AlterColumn<int>(
                name: "taxsystem_id",
                table: "LegalPersons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_LegalPersons_TaxSystems_taxsystem_id",
                table: "LegalPersons",
                column: "taxsystem_id",
                principalTable: "TaxSystems",
                principalColumn: "id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountTypeCodeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpeningBalance",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "AccountTypeCode",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountTypeCode",
                table: "Accounts");

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningBalance",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

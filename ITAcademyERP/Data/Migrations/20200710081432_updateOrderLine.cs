using Microsoft.EntityFrameworkCore.Migrations;

namespace ITAcademyERP.Data.Migrations
{
    public partial class updateOrderLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalNetPrice",
                table: "OrderLine");

            migrationBuilder.DropColumn(
                name: "TotalVatPrice",
                table: "OrderLine");

            migrationBuilder.DropColumn(
                name: "UnitVatPrice",
                table: "OrderLine");

            migrationBuilder.AddColumn<double>(
                name: "vat",
                table: "OrderLine",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vat",
                table: "OrderLine");

            migrationBuilder.AddColumn<double>(
                name: "TotalNetPrice",
                table: "OrderLine",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalVatPrice",
                table: "OrderLine",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UnitVatPrice",
                table: "OrderLine",
                type: "float",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace ITAcademyERP.Data.Migrations
{
    public partial class UpdateOrderPriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderPriorityId",
                table: "OrderHeader",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderPriority",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Priority = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPriority", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeader_OrderPriorityId",
                table: "OrderHeader",
                column: "OrderPriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeader_OrderPriority_OrderPriorityId",
                table: "OrderHeader",
                column: "OrderPriorityId",
                principalTable: "OrderPriority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeader_OrderPriority_OrderPriorityId",
                table: "OrderHeader");

            migrationBuilder.DropTable(
                name: "OrderPriority");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeader_OrderPriorityId",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "OrderPriorityId",
                table: "OrderHeader");
        }
    }
}

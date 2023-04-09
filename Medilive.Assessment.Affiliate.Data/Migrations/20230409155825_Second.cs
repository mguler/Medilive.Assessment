using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medilive.Assessment.Affiliate.Data.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientBlock",
                schema: "UserManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IpNumber = table.Column<long>(type: "bigint", nullable: false),
                    IdentificationCookie = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BlockedUntil = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientBlock", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientBlock",
                schema: "UserManagement");
        }
    }
}

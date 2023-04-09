using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Medilive.Assessment.Affiliate.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UserManagement");

            migrationBuilder.EnsureSchema(
                name: "AuthenticationManagement");

            migrationBuilder.CreateTable(
                name: "Gender",
                schema: "UserManagement",
                columns: table => new
                {
                    Value = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationReferralCodeAudit",
                schema: "UserManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferralCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IdentificationCookie = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IpNumber = table.Column<long>(type: "bigint", nullable: false),
                    AttemptOn = table.Column<long>(type: "bigint", nullable: false),
                    IsSuccessfull = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationReferralCodeAudit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteAccess",
                schema: "AuthenticationManagement",
                columns: table => new
                {
                    Value = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteAccess", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                schema: "UserManagement",
                columns: table => new
                {
                    Value = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "AffiliateUser",
                schema: "UserManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(16)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffiliateUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffiliateUser_Gender_Gender",
                        column: x => x.Gender,
                        principalSchema: "UserManagement",
                        principalTable: "Gender",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Route",
                schema: "AuthenticationManagement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Controller = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    RouteTemplate = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Access = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Route_RouteAccess_Access",
                        column: x => x.Access,
                        principalSchema: "AuthenticationManagement",
                        principalTable: "RouteAccess",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "UserManagement",
                table: "Gender",
                columns: new[] { "Value", "Id", "Name" },
                values: new object[,]
                {
                    { "FEMALE", 1, "Kadin" },
                    { "MALE", 2, "Erkek" },
                    { "NONE", 0, "Secilmemis" }
                });

            migrationBuilder.InsertData(
                schema: "AuthenticationManagement",
                table: "RouteAccess",
                columns: new[] { "Value", "Id", "Name" },
                values: new object[,]
                {
                    { "AUTHENTICATED_USER", 1, "Authenticated" },
                    { "NONE", 0, "Undefined" },
                    { "UNAUTHENTICATED_USER", 2, "Unauthenticated" }
                });

            migrationBuilder.InsertData(
                schema: "UserManagement",
                table: "UserType",
                columns: new[] { "Value", "Id", "Name" },
                values: new object[,]
                {
                    { "ADMINISTRATOR", 2, "Administrator" },
                    { "NORMAL_USER", 1, "Normal User" }
                });

            migrationBuilder.InsertData(
                schema: "UserManagement",
                table: "AffiliateUser",
                columns: new[] { "Id", "Email", "Gender", "IsDeleted", "Lastname", "Name", "Password", "Phone", "UserType", "Username" },
                values: new object[] { 1L, "admin@host.com", "NONE", false, "Administrator", "System", "BV42COTpekZ3tmYsOsJG9jcgIeO78pQ9ERQeQCvFkfI=", "5321111111", "ADMINISTRATOR", "Administrator" });

            migrationBuilder.InsertData(
                schema: "AuthenticationManagement",
                table: "Route",
                columns: new[] { "Id", "Access", "Action", "Controller", "IsDeleted", "RouteTemplate" },
                values: new object[,]
                {
                    { 1L, "UNAUTHENTICATED_USER", "Index", "Home", false, "/" },
                    { 2L, "NONE", "Privacy", "Home", false, "/privacy" },
                    { 3L, "NONE", "GetGenderList", "ReferenceData", false, "/gender-list" },
                    { 4L, "UNAUTHENTICATED_USER", "Register", "User", false, "/new-user-registration" },
                    { 5L, "AUTHENTICATED_USER", "UserHome", "User", false, "/user-home" },
                    { 6L, "AUTHENTICATED_USER", "GetUserinfo", "User", false, "/get-user-info" },
                    { 7L, "UNAUTHENTICATED_USER", "Login", "Authentication", false, "/user-login" },
                    { 8L, "AUTHENTICATED_USER", "Logout", "Authentication", false, "/user-logout" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateUser_Gender",
                schema: "UserManagement",
                table: "AffiliateUser",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Route_Access",
                schema: "AuthenticationManagement",
                table: "Route",
                column: "Access");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AffiliateUser",
                schema: "UserManagement");

            migrationBuilder.DropTable(
                name: "RegistrationReferralCodeAudit",
                schema: "UserManagement");

            migrationBuilder.DropTable(
                name: "Route",
                schema: "AuthenticationManagement");

            migrationBuilder.DropTable(
                name: "UserType",
                schema: "UserManagement");

            migrationBuilder.DropTable(
                name: "Gender",
                schema: "UserManagement");

            migrationBuilder.DropTable(
                name: "RouteAccess",
                schema: "AuthenticationManagement");
        }
    }
}

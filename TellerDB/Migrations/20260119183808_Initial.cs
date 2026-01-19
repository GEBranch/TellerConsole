using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TellerDB.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<int>(type: "int", nullable: false),
                    Street1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Street2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    ZipPlusFour = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Members_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressId", "AccountNumber", "City", "IsActive", "State", "Street1", "Street2", "ZipCode", "ZipPlusFour" },
                values: new object[,]
                {
                    { 1, 1, "Indianapolis", true, "IN", "123 Main St", null, 46201, null },
                    { 2, 2, "New York", true, "IN", "456 Elm St", null, 46038, null },
                    { 3, 3, "Los Angeles", true, "IN", "789 Oak St", null, 29572, null }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "AccountNumber", "AddressId", "FirstName", "IsActive", "LastName" },
                values: new object[,]
                {
                    { 1, 1, 1, "John", true, "Doe" },
                    { 2, 2, 2, "Jane", true, "Smith" },
                    { 3, 3, 3, "Super", true, "Man" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountId", "AccountNumber", "AccountType", "Balance", "IsActive", "MemberId" },
                values: new object[,]
                {
                    { 1, 1, 1, 500.00m, true, 1 },
                    { 2, 1, 2, 1000.00m, true, 1 },
                    { 3, 2, 1, 5000.00m, true, 2 },
                    { 4, 3, 1, 1500.00m, true, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_MemberId",
                table: "Accounts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_AddressId",
                table: "Members",
                column: "AddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}

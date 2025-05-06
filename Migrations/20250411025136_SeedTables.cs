using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quorom.Migrations
{
    /// <inheritdoc />
    public partial class SeedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MembershipTypes",
                columns: table => new
                {
                    MembershipTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipTypes", x => x.MembershipTypeId);
                });

            migrationBuilder.CreateTable(
                name: "InitiativeBoard",
                columns: table => new
                {
                    InitiativeBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiativeBoard", x => x.InitiativeBoardId);
                    table.ForeignKey(
                        name: "FK_InitiativeBoard_Initiatives_InitiativeId",
                        column: x => x.InitiativeId,
                        principalTable: "Initiatives",
                        principalColumn: "InitiativeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InitiativeBoard_MembershipTypes_MembershipTypeId",
                        column: x => x.MembershipTypeId,
                        principalTable: "MembershipTypes",
                        principalColumn: "MembershipTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InitiativeBoard_Recipients_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Recipients",
                        principalColumn: "RecipientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "070db657-d8c4-491c-ad56-056f137e11bd", "AQAAAAIAAYagAAAAEOl2IOhk88Gp9QyUQ6Au2nF6TIVlH37JkR52A1nWnYIyQqT2ulKGTlNgjMgY54F/lg==", "4a46b61f-93cb-4ab3-8555-9f96eef4c6d4" });

            migrationBuilder.InsertData(
                table: "MembershipTypes",
                columns: new[] { "MembershipTypeId", "CreatedByUserId", "CreatedOnDateTime", "DeletedByUserId", "DeletedOnDateTime", "Description", "IsActive", "IsDeleted", "Title", "UpdatedByUserId", "UpdatedOnDateTime" },
                values: new object[,]
                {
                    { new Guid("0b999285-c5e4-4378-8a80-699f3a06316f"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Secretary", true, false, "Secretary", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("233de29d-df9f-4557-81f8-2952f62ea25f"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Chairman", true, false, "Chairman", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("28dae33c-bc56-45cd-a11e-d2cad9332d7c"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Director", true, false, "Director", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b97d931c-36be-4389-b721-442380e05c04"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Executive Assistant", true, false, "Executive Assistant", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("ef33fcc1-7e7c-4d84-9ef8-5b048285af8f"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Member", true, false, "Member", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InitiativeBoard_InitiativeId",
                table: "InitiativeBoard",
                column: "InitiativeId");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativeBoard_MembershipTypeId",
                table: "InitiativeBoard",
                column: "MembershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativeBoard_RecipientId",
                table: "InitiativeBoard",
                column: "RecipientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InitiativeBoard");

            migrationBuilder.DropTable(
                name: "MembershipTypes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "616f7a25-15d4-493a-a5c1-c6e0b31824de", "AQAAAAIAAYagAAAAEG3BOUh7ra/rf+8klWFFIWr3fKgPp2yzZF2+s75uvYFQ2srcHkb4NJlxf6ybezy3cg==", "0121226d-f062-414f-b251-f504a712c6d8" });
        }
    }
}

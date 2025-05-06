using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quorom.Migrations
{
    /// <inheritdoc />
    public partial class MOreUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cda2c2c5-058f-4504-bd54-7d28e2ed5974", "AQAAAAIAAYagAAAAEFwP7Meam4/h9Q+wTiJG3g52AKWY428nql4na3SO/jQKim6lBhRvCX6Dp0ypbf+EQQ==", "36d67715-87fc-4593-be00-77da9fa8414c" });

            migrationBuilder.InsertData(
                table: "TaskTypes",
                columns: new[] { "TaskTypeId", "CreatedByUserId", "CreatedOnDateTime", "DeletedByUserId", "DeletedOnDateTime", "Description", "IsActive", "IsDeleted", "Title", "UpdatedByUserId", "UpdatedOnDateTime" },
                values: new object[,]
                {
                    { new Guid("3843a8a3-5242-494b-97f6-e5f8bef8dac6"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Incidental", true, false, "Incidental", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("410b8c41-fea2-481d-b129-ae4fe6e04c08"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Approval", true, false, "Approval", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("8cbb73c4-e357-4593-a157-248dd0039f3a"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Follow-Up", true, false, "Follow-Up", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("9baba539-467d-4668-b2bf-2d8bb9dacf40"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Dependency", true, false, "Dependency", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e7642e73-5b67-4c8e-b1bb-aab4043375bb"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Coordinated", true, false, "Coordinated", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("fb74d606-84f4-4812-aa8b-07a49d0a94fc"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Planned", true, false, "Planned", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskTypes",
                keyColumn: "TaskTypeId",
                keyValue: new Guid("3843a8a3-5242-494b-97f6-e5f8bef8dac6"));

            migrationBuilder.DeleteData(
                table: "TaskTypes",
                keyColumn: "TaskTypeId",
                keyValue: new Guid("410b8c41-fea2-481d-b129-ae4fe6e04c08"));

            migrationBuilder.DeleteData(
                table: "TaskTypes",
                keyColumn: "TaskTypeId",
                keyValue: new Guid("8cbb73c4-e357-4593-a157-248dd0039f3a"));

            migrationBuilder.DeleteData(
                table: "TaskTypes",
                keyColumn: "TaskTypeId",
                keyValue: new Guid("9baba539-467d-4668-b2bf-2d8bb9dacf40"));

            migrationBuilder.DeleteData(
                table: "TaskTypes",
                keyColumn: "TaskTypeId",
                keyValue: new Guid("e7642e73-5b67-4c8e-b1bb-aab4043375bb"));

            migrationBuilder.DeleteData(
                table: "TaskTypes",
                keyColumn: "TaskTypeId",
                keyValue: new Guid("fb74d606-84f4-4812-aa8b-07a49d0a94fc"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4800e483-0fce-4b0f-90ba-32cbdf34de46", "AQAAAAIAAYagAAAAEGOQmiKlbMpv87tT89LrMZVET59QMsIvhpwCO7bjnDdqTB9sFHlqK4FeyiIZHSqo+Q==", "d0c7a3b5-9839-42cd-8e4d-59597f346f9b" });
        }
    }
}

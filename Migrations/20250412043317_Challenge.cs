using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quorom.Migrations
{
    /// <inheritdoc />
    public partial class Challenge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Challenges");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "35ee2fbe-f5a2-4a59-a128-67d5f3fe5e19", "AQAAAAIAAYagAAAAELkNIWwjQkRdzBYJ4r3Pl1LFAIPYvvoeEbBQ1UHlIX31pc/5dsdSqQVUI6DTYtG8pw==", "f50ec2cb-e33d-4261-b240-9a2fde2d3516" });

            migrationBuilder.InsertData(
                table: "ChallengeTypes",
                columns: new[] { "ChallengeTypeId", "CreatedByUserId", "CreatedOnDateTime", "DeletedByUserId", "DeletedOnDateTime", "Description", "IsActive", "IsDeleted", "Title", "UpdatedByUserId", "UpdatedOnDateTime" },
                values: new object[,]
                {
                    { new Guid("109b6f41-e097-4b86-9fec-b50d5d8c41cc"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Budget", true, false, "Budget", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("15946387-7783-480d-b7e6-c7e1729c4bd6"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Goal", true, false, "Goal", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("2701c191-a3f4-4d01-8382-2ab7820d67cf"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Scheduling", true, false, "Scheduling", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("2ee65cee-fd07-45f1-83ef-347c64da6ef5"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Stakeholder", true, false, "Stakeholder", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3992af8b-fffa-4461-ba9b-7bb6b0d93d32"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ambiguity", true, false, "Ambiguity", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3adb434e-7cb6-447b-af2d-2834a69d0f74"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Technology", true, false, "Technology", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("584220b7-26e4-4a9e-8333-e86bfea55794"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Deadline", true, false, "Deadline", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("5877bd14-a5f3-42d3-957f-2f44661d48f4"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Resource", true, false, "Resource", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7d19481f-6bcc-41ae-8345-ca2ce0aef87b"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Conflict", true, false, "Conflict", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("86821e87-4ff8-4f36-9d26-da6e5ea31db3"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Competency", true, false, "Competency", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("89733f5b-c28a-43b2-9dc2-14466e5a37b5"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Accountability", true, false, "Accountability", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("89a2201f-5e96-4adb-b6d6-1b2a2091f6a5"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Risk", true, false, "Risk", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("94d94923-3a98-46f1-a4c2-cf04f4833dde"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Communication", true, false, "Communication", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("be252507-8103-43df-b7d3-2f5ea1bc4d46"), "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Scope Creep", true, false, "Scope Creep", "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("109b6f41-e097-4b86-9fec-b50d5d8c41cc"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("15946387-7783-480d-b7e6-c7e1729c4bd6"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("2701c191-a3f4-4d01-8382-2ab7820d67cf"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("2ee65cee-fd07-45f1-83ef-347c64da6ef5"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("3992af8b-fffa-4461-ba9b-7bb6b0d93d32"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("3adb434e-7cb6-447b-af2d-2834a69d0f74"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("584220b7-26e4-4a9e-8333-e86bfea55794"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("5877bd14-a5f3-42d3-957f-2f44661d48f4"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("7d19481f-6bcc-41ae-8345-ca2ce0aef87b"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("86821e87-4ff8-4f36-9d26-da6e5ea31db3"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("89733f5b-c28a-43b2-9dc2-14466e5a37b5"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("89a2201f-5e96-4adb-b6d6-1b2a2091f6a5"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("94d94923-3a98-46f1-a4c2-cf04f4833dde"));

            migrationBuilder.DeleteData(
                table: "ChallengeTypes",
                keyColumn: "ChallengeTypeId",
                keyValue: new Guid("be252507-8103-43df-b7d3-2f5ea1bc4d46"));

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Challenges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cda2c2c5-058f-4504-bd54-7d28e2ed5974", "AQAAAAIAAYagAAAAEFwP7Meam4/h9Q+wTiJG3g52AKWY428nql4na3SO/jQKim6lBhRvCX6Dp0ypbf+EQQ==", "36d67715-87fc-4593-be00-77da9fa8414c" });
        }
    }
}

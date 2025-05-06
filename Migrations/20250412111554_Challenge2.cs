using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quorom.Migrations
{
    /// <inheritdoc />
    public partial class Challenge2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskChallenges",
                columns: table => new
                {
                    TaskChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TaskChallenges", x => x.TaskChallengeId);
                    table.ForeignKey(
                        name: "FK_TaskChallenges_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "ChallengeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskChallenges_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bad296a9-ee03-4a0b-9067-82ebecc815be", "AQAAAAIAAYagAAAAEMlRElWtvkS6ABz3VmaO/eI8G2Ro/TmUQSlcHZFRNtGdXerE5NYsmbX6nHgqOnj0jA==", "ac70f996-f1ad-494a-a87b-44ec6996a2be" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskChallenges_ChallengeId",
                table: "TaskChallenges",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskChallenges_TaskId",
                table: "TaskChallenges",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskChallenges");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "35ee2fbe-f5a2-4a59-a128-67d5f3fe5e19", "AQAAAAIAAYagAAAAELkNIWwjQkRdzBYJ4r3Pl1LFAIPYvvoeEbBQ1UHlIX31pc/5dsdSqQVUI6DTYtG8pw==", "f50ec2cb-e33d-4261-b240-9a2fde2d3516" });
        }
    }
}

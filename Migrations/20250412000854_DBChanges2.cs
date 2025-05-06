using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quorom.Migrations
{
    /// <inheritdoc />
    public partial class DBChanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InitiativeBoard");

            migrationBuilder.DropTable(
                name: "NotificationGroupRecipients");

            migrationBuilder.DropTable(
                name: "MembershipTypes");

            migrationBuilder.DropTable(
                name: "Recipients");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubStatus",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InitiativeTasks",
                columns: table => new
                {
                    InitiativeTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_InitiativeTasks", x => x.InitiativeTaskId);
                    table.ForeignKey(
                        name: "FK_InitiativeTasks_Initiatives_InitiativeId",
                        column: x => x.InitiativeId,
                        principalTable: "Initiatives",
                        principalColumn: "InitiativeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InitiativeTasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quoromites",
                columns: table => new
                {
                    QuoromiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_Quoromites", x => x.QuoromiteId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationGroupQuoromites",
                columns: table => new
                {
                    NotificationGroupQuoromiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuoromiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_NotificationGroupQuoromites", x => x.NotificationGroupQuoromiteId);
                    table.ForeignKey(
                        name: "FK_NotificationGroupQuoromites_NotificationGroups_NotificationGroupId",
                        column: x => x.NotificationGroupId,
                        principalTable: "NotificationGroups",
                        principalColumn: "NotificationGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationGroupQuoromites_Quoromites_QuoromiteId",
                        column: x => x.QuoromiteId,
                        principalTable: "Quoromites",
                        principalColumn: "QuoromiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuoromiteTask",
                columns: table => new
                {
                    QuoromitesQuoromiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TasksTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoromiteTask", x => new { x.QuoromitesQuoromiteId, x.TasksTaskId });
                    table.ForeignKey(
                        name: "FK_QuoromiteTask_Quoromites_QuoromitesQuoromiteId",
                        column: x => x.QuoromitesQuoromiteId,
                        principalTable: "Quoromites",
                        principalColumn: "QuoromiteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuoromiteTask_Tasks_TasksTaskId",
                        column: x => x.TasksTaskId,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4800e483-0fce-4b0f-90ba-32cbdf34de46", "AQAAAAIAAYagAAAAEGOQmiKlbMpv87tT89LrMZVET59QMsIvhpwCO7bjnDdqTB9sFHlqK4FeyiIZHSqo+Q==", "d0c7a3b5-9839-42cd-8e4d-59597f346f9b" });

            migrationBuilder.CreateIndex(
                name: "IX_InitiativeTasks_InitiativeId",
                table: "InitiativeTasks",
                column: "InitiativeId");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativeTasks_TaskId",
                table: "InitiativeTasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationGroupQuoromites_NotificationGroupId",
                table: "NotificationGroupQuoromites",
                column: "NotificationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationGroupQuoromites_QuoromiteId",
                table: "NotificationGroupQuoromites",
                column: "QuoromiteId");

            migrationBuilder.CreateIndex(
                name: "IX_QuoromiteTask_TasksTaskId",
                table: "QuoromiteTask",
                column: "TasksTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InitiativeTasks");

            migrationBuilder.DropTable(
                name: "NotificationGroupQuoromites");

            migrationBuilder.DropTable(
                name: "QuoromiteTask");

            migrationBuilder.DropTable(
                name: "Quoromites");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SubStatus",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "MembershipTypes",
                columns: table => new
                {
                    MembershipTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipTypes", x => x.MembershipTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Recipients",
                columns: table => new
                {
                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.RecipientId);
                });

            migrationBuilder.CreateTable(
                name: "InitiativeBoard",
                columns: table => new
                {
                    InitiativeBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "NotificationGroupRecipients",
                columns: table => new
                {
                    NotificationGroupRecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationGroupRecipients", x => x.NotificationGroupRecipientId);
                    table.ForeignKey(
                        name: "FK_NotificationGroupRecipients_NotificationGroups_NotificationGroupId",
                        column: x => x.NotificationGroupId,
                        principalTable: "NotificationGroups",
                        principalColumn: "NotificationGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationGroupRecipients_Recipients_RecipientId",
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

            migrationBuilder.CreateIndex(
                name: "IX_NotificationGroupRecipients_NotificationGroupId",
                table: "NotificationGroupRecipients",
                column: "NotificationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationGroupRecipients_RecipientId",
                table: "NotificationGroupRecipients",
                column: "RecipientId");
        }
    }
}

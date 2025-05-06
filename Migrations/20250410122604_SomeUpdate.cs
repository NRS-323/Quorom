using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quorom.Migrations
{
    /// <inheritdoc />
    public partial class SomeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Initiatives",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Objective",
                table: "Initiatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Initiatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d4ba709c-9914-4c17-a8ea-437f03777371", "AQAAAAIAAYagAAAAEC0mfbeguekgZfTqME1qAf8ZwtFNIModr3ZTLmwSFO7D7q4RNuvFIqamXzBSj+2FDg==", "d9213b8d-e229-43e9-87b6-4d95eac7fc48" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Objective",
                table: "Initiatives");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Initiatives");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Initiatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "04df40e8-9f90-4bcd-83a2-c3a57bf7abd5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "105fed3d-7a6b-418c-87e6-9da776625d96", "AQAAAAIAAYagAAAAENDzaraUhePCZjtXNJ6PFzyW7b/kuzGV1DNKGuEUHzZHnuPq51R/SciiBOb6ipmkmg==", "2a504cfb-4db5-464c-a32f-a0b4dc3e25d7" });
        }
    }
}

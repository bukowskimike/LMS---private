using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class AddDeadLineToDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileLocationURL",
                table: "Document");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Document",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "DocumentViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Feedback = table.Column<string>(nullable: true),
                    UploadDateTime = table.Column<DateTime>(nullable: false),
                    Deadline = table.Column<DateTime>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    CourseId = table.Column<int>(nullable: true),
                    ModuleId = table.Column<int>(nullable: true),
                    ActivityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentViewModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentViewModel");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Document");

            migrationBuilder.AddColumn<string>(
                name: "FileLocationURL",
                table: "Document",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class CreateDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Feedback = table.Column<string>(nullable: true),
                    UploadDateTime = table.Column<DateTime>(nullable: false),
                    FileLocationURL = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<int>(nullable: true),
                    ModuleId = table.Column<int>(nullable: true),
                    ActivityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Document_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Document_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Document_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DocumentId",
                table: "AspNetUsers",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_ActivityId",
                table: "Document",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_CourseId",
                table: "Document",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_ModuleId",
                table: "Document",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Document_DocumentId",
                table: "AspNetUsers",
                column: "DocumentId",
                principalTable: "Document",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Document_DocumentId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DocumentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "AspNetUsers");
        }
    }
}

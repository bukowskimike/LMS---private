using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS.Migrations
{
    public partial class GuidToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Document_DocumentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DocumentId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Document",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<int>(
                name: "DocumentId1",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DocumentId1",
                table: "AspNetUsers",
                column: "DocumentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Document_DocumentId1",
                table: "AspNetUsers",
                column: "DocumentId1",
                principalTable: "Document",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Document_DocumentId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DocumentId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DocumentId1",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId",
                table: "Document",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DocumentId",
                table: "AspNetUsers",
                column: "DocumentId",
                unique: true,
                filter: "[DocumentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Document_DocumentId",
                table: "AspNetUsers",
                column: "DocumentId",
                principalTable: "Document",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RazorBlog.Data.Migrations
{
    public partial class TopicNameIndex_BlogDescriptionRenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityModeration_AspNetUsers_ModeratorId",
                table: "CommunityModeration");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityModeration_Topic_CommunityId",
                table: "CommunityModeration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunityModeration",
                table: "CommunityModeration");

            migrationBuilder.RenameTable(
                name: "CommunityModeration",
                newName: "ModeratorAssignment");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Blog",
                newName: "Introduction");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityModeration_ModeratorId",
                table: "ModeratorAssignment",
                newName: "IX_ModeratorAssignment_ModeratorId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Topic",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "ViewCount",
                table: "Blog",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModeratorAssignment",
                table: "ModeratorAssignment",
                columns: new[] { "CommunityId", "ModeratorId" });

            migrationBuilder.CreateIndex(
                name: "IX_Topic_Name",
                table: "Topic",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ModeratorAssignment_AspNetUsers_ModeratorId",
                table: "ModeratorAssignment",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModeratorAssignment_Topic_CommunityId",
                table: "ModeratorAssignment",
                column: "CommunityId",
                principalTable: "Topic",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeratorAssignment_AspNetUsers_ModeratorId",
                table: "ModeratorAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeratorAssignment_Topic_CommunityId",
                table: "ModeratorAssignment");

            migrationBuilder.DropIndex(
                name: "IX_Topic_Name",
                table: "Topic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModeratorAssignment",
                table: "ModeratorAssignment");

            migrationBuilder.RenameTable(
                name: "ModeratorAssignment",
                newName: "CommunityModeration");

            migrationBuilder.RenameColumn(
                name: "Introduction",
                table: "Blog",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_ModeratorAssignment_ModeratorId",
                table: "CommunityModeration",
                newName: "IX_CommunityModeration_ModeratorId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Topic",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "ViewCount",
                table: "Blog",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunityModeration",
                table: "CommunityModeration",
                columns: new[] { "CommunityId", "ModeratorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityModeration_AspNetUsers_ModeratorId",
                table: "CommunityModeration",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityModeration_Topic_CommunityId",
                table: "CommunityModeration",
                column: "CommunityId",
                principalTable: "Topic",
                principalColumn: "Id");
        }
    }
}

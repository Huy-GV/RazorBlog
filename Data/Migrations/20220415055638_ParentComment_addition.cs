using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RazorBlog.Data.Migrations
{
    public partial class ParentComment_addition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Community_AspNetUsers_CreatorUserId",
                table: "Community");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityModeration_Community_CommunityId",
                table: "CommunityModeration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Community",
                table: "Community");

            migrationBuilder.RenameTable(
                name: "Community",
                newName: "Topic");

            migrationBuilder.RenameIndex(
                name: "IX_Community_CreatorUserId",
                table: "Topic",
                newName: "IX_Topic_CreatorUserId");

            migrationBuilder.AddColumn<int>(
                name: "ParentCommentId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topic",
                table: "Topic",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentCommentId",
                table: "Comment",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_TopicId",
                table: "Blog",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_Topic_TopicId",
                table: "Blog",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_ParentCommentId",
                table: "Comment",
                column: "ParentCommentId",
                principalTable: "Comment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityModeration_Topic_CommunityId",
                table: "CommunityModeration",
                column: "CommunityId",
                principalTable: "Topic",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_AspNetUsers_CreatorUserId",
                table: "Topic",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_Topic_TopicId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_ParentCommentId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityModeration_Topic_CommunityId",
                table: "CommunityModeration");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_AspNetUsers_CreatorUserId",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ParentCommentId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Blog_TopicId",
                table: "Blog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topic",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Topic",
                newName: "Community");

            migrationBuilder.RenameIndex(
                name: "IX_Topic_CreatorUserId",
                table: "Community",
                newName: "IX_Community_CreatorUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Community",
                table: "Community",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Community_AspNetUsers_CreatorUserId",
                table: "Community",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityModeration_Community_CommunityId",
                table: "CommunityModeration",
                column: "CommunityId",
                principalTable: "Community",
                principalColumn: "Id");
        }
    }
}
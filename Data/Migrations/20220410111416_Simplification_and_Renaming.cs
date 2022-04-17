using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RazorBlog.Data.Migrations
{
    public partial class Simplification_and_Renaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_AspNetUsers_AppUserID",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_AppUserID",
                table: "Comment");

            migrationBuilder.DropTable(
                name: "CommunitySubscription");

            migrationBuilder.DropTable(
                name: "ModerationHistory");

            migrationBuilder.RenameColumn(
                name: "AppUserID",
                table: "Comment",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_AppUserID",
                table: "Comment",
                newName: "IX_Comment_AppUserId");

            migrationBuilder.RenameColumn(
                name: "AppUserID",
                table: "Blog",
                newName: "AppUserId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Blog",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Blog_AppUserID",
                table: "Blog",
                newName: "IX_Blog_AppUserId");

            migrationBuilder.RenameColumn(
                name: "CommunityId",
                table: "BanTicket",
                newName: "TopicId");

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Blog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_AspNetUsers_AppUserId",
                table: "Blog",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_AppUserId",
                table: "Comment",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_AspNetUsers_AppUserId",
                table: "Blog");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_AppUserId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Blog");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Comment",
                newName: "AppUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_AppUserId",
                table: "Comment",
                newName: "IX_Comment_AppUserID");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Blog",
                newName: "AppUserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Blog",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Blog_AppUserId",
                table: "Blog",
                newName: "IX_Blog_AppUserID");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "BanTicket",
                newName: "CommunityId");

            migrationBuilder.CreateTable(
                name: "CommunitySubscription",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunitySubscription", x => new { x.UserId, x.CommunityId });
                    table.ForeignKey(
                        name: "FK_CommunitySubscription_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunitySubscription_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModerationHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModerationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModerationHistory_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunitySubscription_CommunityId",
                table: "CommunitySubscription",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationHistory_CommunityId",
                table: "ModerationHistory",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_AspNetUsers_AppUserID",
                table: "Blog",
                column: "AppUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_AppUserID",
                table: "Comment",
                column: "AppUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
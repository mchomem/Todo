using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Todo.Core.Models.DataBase.Migrations
{
    public partial class AddPictureInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPicture",
                columns: table => new
                {
                    UserPictureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PictureFromUserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPicture", x => x.UserPictureID);
                    table.ForeignKey(
                        name: "FK_UserPicture_User_PictureFromUserID",
                        column: x => x.PictureFromUserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPicture_PictureFromUserID",
                table: "UserPicture",
                column: "PictureFromUserID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPicture");
        }
    }
}

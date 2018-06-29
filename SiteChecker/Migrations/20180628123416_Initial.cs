using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteChecker.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlCheckTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    LastCheckResultJson = table.Column<string>(nullable: true),
                    LastCheckDateTime = table.Column<DateTime>(nullable: true),
                    HttpStatusCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlCheckTasks", x => x.Id);
                    table.UniqueConstraint("AK_UrlCheckTasks_Url", x => x.Url);
                });

            migrationBuilder.CreateTable(
                name: "UrlCheckTaskResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UrlCheckTaskId = table.Column<int>(nullable: false),
                    CheckDateTime = table.Column<DateTime>(nullable: false),
                    LastCheckResultJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlCheckTaskResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlCheckTaskResults_UrlCheckTasks_UrlCheckTaskId",
                        column: x => x.UrlCheckTaskId,
                        principalTable: "UrlCheckTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlCheckTaskResults_UrlCheckTaskId",
                table: "UrlCheckTaskResults",
                column: "UrlCheckTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlCheckTaskResults");

            migrationBuilder.DropTable(
                name: "UrlCheckTasks");
        }
    }
}

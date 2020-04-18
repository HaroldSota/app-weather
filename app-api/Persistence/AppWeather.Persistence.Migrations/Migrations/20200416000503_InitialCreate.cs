using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppWeather.Persistence.Migrations.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSearch",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(maxLength: 36, nullable: false),
                    CityName = table.Column<string>(maxLength: 50, nullable: false),
                    Temperature = table.Column<float>(nullable: true),
                    Humidity = table.Column<int>(nullable: true),
                    SearchTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSearch", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSearch");
        }
    }
}

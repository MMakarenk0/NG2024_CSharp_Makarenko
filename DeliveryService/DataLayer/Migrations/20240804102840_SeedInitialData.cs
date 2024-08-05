using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name", "Phone" },
                values: new object[] { new Guid("0d9e0598-94ce-4e79-8580-ab51f84dd144"), "Kyle", "2222" });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name", "Phone" },
                values: new object[] { new Guid("cecbc188-34d4-48d9-8088-f6315a091493"), "John", "1111" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("0d9e0598-94ce-4e79-8580-ab51f84dd144"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("cecbc188-34d4-48d9-8088-f6315a091493"));
        }
    }
}

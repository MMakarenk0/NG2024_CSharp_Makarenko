using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    public partial class AddDateAndIsReveivedToItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemCategories",
                keyColumns: new[] { "CategoryId", "ItemId" },
                keyValues: new object[] { new Guid("710f409a-fec3-4dea-b561-60dcd302b65b"), new Guid("81509cc6-0481-4979-b279-1f6d7263e43c") });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("710f409a-fec3-4dea-b561-60dcd302b65b"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("81509cc6-0481-4979-b279-1f6d7263e43c"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("a1cd570b-7d18-4f5b-84fe-fc54045b2b78"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("debd53b3-c423-4127-a669-e39d1f06b301"));

            migrationBuilder.DeleteData(
                table: "Storages",
                keyColumn: "Id",
                keyValue: new Guid("1591f96b-e048-46c4-9a23-ba3beff8fafe"));

            migrationBuilder.DeleteData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: new Guid("0afcfdf2-62e5-4ef2-b8b7-2822575e53e4"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "isReceived",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "isReceived",
                table: "Items");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description" },
                values: new object[] { new Guid("710f409a-fec3-4dea-b561-60dcd302b65b"), "Electronics" });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name", "Phone" },
                values: new object[,]
                {
                    { new Guid("a1cd570b-7d18-4f5b-84fe-fc54045b2b78"), "John", "1111" },
                    { new Guid("debd53b3-c423-4127-a669-e39d1f06b301"), "Kyle", "2222" }
                });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "Name", "StorageId" },
                values: new object[] { new Guid("0afcfdf2-62e5-4ef2-b8b7-2822575e53e4"), "Michael", new Guid("1591f96b-e048-46c4-9a23-ba3beff8fafe") });

            migrationBuilder.InsertData(
                table: "Storages",
                columns: new[] { "Id", "Address", "DirectorId", "Number" },
                values: new object[] { new Guid("1591f96b-e048-46c4-9a23-ba3beff8fafe"), "456 Maple Ave, Apt 12, Newtown, USA", new Guid("0afcfdf2-62e5-4ef2-b8b7-2822575e53e4"), 235 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "Price", "ReceiverId", "SenderId", "StorageId", "Weight" },
                values: new object[] { new Guid("81509cc6-0481-4979-b279-1f6d7263e43c"), "Laptop ACER Nitro 5", 1200m, new Guid("debd53b3-c423-4127-a669-e39d1f06b301"), new Guid("a1cd570b-7d18-4f5b-84fe-fc54045b2b78"), new Guid("1591f96b-e048-46c4-9a23-ba3beff8fafe"), 2.5f });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "CategoryId", "ItemId", "Id" },
                values: new object[] { new Guid("710f409a-fec3-4dea-b561-60dcd302b65b"), new Guid("81509cc6-0481-4979-b279-1f6d7263e43c"), new Guid("00000000-0000-0000-0000-000000000000") });
        }
    }
}

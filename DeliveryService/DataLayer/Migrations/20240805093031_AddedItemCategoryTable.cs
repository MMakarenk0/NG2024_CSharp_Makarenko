using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    public partial class AddedItemCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryItem");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("0d9e0598-94ce-4e79-8580-ab51f84dd144"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("cecbc188-34d4-48d9-8088-f6315a091493"));

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => new { x.ItemId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ItemCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemCategories_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_CategoryId",
                table: "ItemCategories",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCategories");

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

            migrationBuilder.CreateTable(
                name: "CategoryItem",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryItem", x => new { x.CategoriesId, x.ItemsId });
                    table.ForeignKey(
                        name: "FK_CategoryItem_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryItem_Items_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name", "Phone" },
                values: new object[] { new Guid("0d9e0598-94ce-4e79-8580-ab51f84dd144"), "Kyle", "2222" });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name", "Phone" },
                values: new object[] { new Guid("cecbc188-34d4-48d9-8088-f6315a091493"), "John", "1111" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryItem_ItemsId",
                table: "CategoryItem",
                column: "ItemsId");
        }
    }
}

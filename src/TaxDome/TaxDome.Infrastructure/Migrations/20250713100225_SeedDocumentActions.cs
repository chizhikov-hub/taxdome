using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaxDome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDocumentActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("111232af-ff06-49a5-8c33-eb50de03188a"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("3a803eb3-0002-4de9-ad0e-195de4e6902d"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("52d8f6b2-87f7-428e-8271-a258df6bdccb"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("962705dd-8017-49ca-95c0-cf3e7c0243d1"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("9e464a8c-474e-4eae-a393-c5b41ec748de"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("d95e9065-b80a-49fd-be6c-d36478e4d4b3"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("ebb303c7-6a84-4999-920e-1cdb646ba176"));

            migrationBuilder.InsertData(
                table: "DocumentActions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3be186f1-17a4-42d9-aa8b-cfeba27eecd6"), "Pending Approval" },
                    { new Guid("5c0e250a-fd43-4fd4-a5ec-00575b08edf6"), "Invoice Linked" },
                    { new Guid("7ac3bf0e-5b3d-47de-b0a0-68ac68dfbba2"), "Pending Signature" },
                    { new Guid("7d3b3075-e252-473c-9fd1-a1e7213dc75d"), "Retry" },
                    { new Guid("7d76be70-9b0a-4942-9a89-3a05c99cd907"), "Job Processing" },
                    { new Guid("9fb03bcf-d42e-497d-a756-2ab2cf13c6b7"), "Approved" },
                    { new Guid("ff7b2e8b-93dd-471d-8daf-0ee8c35f4371"), "Job Linked" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("3be186f1-17a4-42d9-aa8b-cfeba27eecd6"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("5c0e250a-fd43-4fd4-a5ec-00575b08edf6"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("7ac3bf0e-5b3d-47de-b0a0-68ac68dfbba2"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("7d3b3075-e252-473c-9fd1-a1e7213dc75d"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("7d76be70-9b0a-4942-9a89-3a05c99cd907"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("9fb03bcf-d42e-497d-a756-2ab2cf13c6b7"));

            migrationBuilder.DeleteData(
                table: "DocumentActions",
                keyColumn: "Id",
                keyValue: new Guid("ff7b2e8b-93dd-471d-8daf-0ee8c35f4371"));

            migrationBuilder.InsertData(
                table: "DocumentActions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("111232af-ff06-49a5-8c33-eb50de03188a"), "Job Processing" },
                    { new Guid("3a803eb3-0002-4de9-ad0e-195de4e6902d"), "Pending Signature" },
                    { new Guid("52d8f6b2-87f7-428e-8271-a258df6bdccb"), "Approved" },
                    { new Guid("962705dd-8017-49ca-95c0-cf3e7c0243d1"), "Retry" },
                    { new Guid("9e464a8c-474e-4eae-a393-c5b41ec748de"), "Invoice Linked" },
                    { new Guid("d95e9065-b80a-49fd-be6c-d36478e4d4b3"), "Pending Approval" },
                    { new Guid("ebb303c7-6a84-4999-920e-1cdb646ba176"), "Job Linked" }
                });
        }
    }
}

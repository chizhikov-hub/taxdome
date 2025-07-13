using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaxDome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedClientsAndFolders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0888d441-f5fd-403f-a43a-fd7fa5d6bca2"), "Leslie Alexander" },
                    { new Guid("9baab71d-8a1c-413b-93fe-b996eba47c6d"), "Esther Howard" },
                    { new Guid("a3cac9c5-c924-444f-a4c0-38823e3c9bd6"), "Jane Cooper" }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("c5dac252-7961-4638-bfbe-8291ef99d3c7"), "Shared with Client" },
                    { new Guid("f796ef33-5b93-40fd-9eb5-d34668843f45"), "Private" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("0888d441-f5fd-403f-a43a-fd7fa5d6bca2"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("9baab71d-8a1c-413b-93fe-b996eba47c6d"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("a3cac9c5-c924-444f-a4c0-38823e3c9bd6"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("c5dac252-7961-4638-bfbe-8291ef99d3c7"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("f796ef33-5b93-40fd-9eb5-d34668843f45"));
        }
    }
}

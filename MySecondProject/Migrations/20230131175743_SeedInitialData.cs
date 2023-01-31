using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySecondProject.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[] { 1, "smeldoc@gmail.com", "Andriy", "PIrgcQjPgUpuyF8l+7CEo2bT+eebTyKYc+f1fDoGjLs=" });

            migrationBuilder.InsertData(
                table: "CustomLists",
                columns: new[] { "Id", "IsDeleted", "Name", "UserId" },
                values: new object[] { 1, false, "List 1", 1 });

            migrationBuilder.InsertData(
                table: "ToDoTasks",
                columns: new[] { "Id", "CreationDate", "CustomListId", "Description", "DueToDate", "Importance", "IsCompleted", "IsDeleted", "IsFavorite", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 31, 19, 57, 43, 465, DateTimeKind.Local).AddTicks(9215), null, "I have to go shopping", new DateTime(2023, 2, 4, 0, 0, 0, 0, DateTimeKind.Local), 0, false, false, false, "Shop", 1 },
                    { 2, new DateTime(2023, 1, 31, 19, 57, 43, 465, DateTimeKind.Local).AddTicks(9248), null, "I have to learn English", new DateTime(2023, 2, 15, 0, 0, 0, 0, DateTimeKind.Local), 1, false, false, false, "Learning", 1 },
                    { 3, new DateTime(2023, 1, 31, 19, 57, 43, 465, DateTimeKind.Local).AddTicks(9252), null, "I have to learn Asp.Net Core", new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Local), 2, false, false, true, "AspNet", 1 }
                });

            migrationBuilder.InsertData(
                table: "ToDoTasks",
                columns: new[] { "Id", "CreationDate", "CustomListId", "Description", "DueToDate", "Importance", "IsCompleted", "IsDeleted", "IsFavorite", "Title", "UserId" },
                values: new object[] { 4, new DateTime(2023, 1, 30, 19, 57, 43, 465, DateTimeKind.Local).AddTicks(9257), 1, "Description", new DateTime(2023, 1, 31, 19, 57, 43, 465, DateTimeKind.Local).AddTicks(9259), 2, false, false, true, "CustomList Task1", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ToDoTasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ToDoTasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ToDoTasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ToDoTasks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CustomLists",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}

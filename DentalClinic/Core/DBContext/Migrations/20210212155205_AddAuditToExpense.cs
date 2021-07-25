using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DBContext.Migrations
{
    public partial class AddAuditToExpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descriptoin",
                table: "Expense");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Expense",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Expense",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Expense",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Expense",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Expense",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Expense");

            migrationBuilder.AddColumn<string>(
                name: "Descriptoin",
                table: "Expense",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

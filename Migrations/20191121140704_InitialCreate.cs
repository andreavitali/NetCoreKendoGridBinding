using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreKendoAngularGridBinding.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    Salary = table.Column<int>(nullable: false),
                    Recruitment = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employee_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "DepartmentID", "Description" },
                values: new object[] { 1, "R&D" });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "DepartmentID", "Description" },
                values: new object[] { 2, "Accounting" });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeID", "DepartmentId", "FirstName", "LastName", "Recruitment", "Salary" },
                values: new object[] { 1, 1, "Bill", "Jones", new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000 });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeID", "DepartmentId", "FirstName", "LastName", "Recruitment", "Salary" },
                values: new object[] { 2, 1, "Rob", "Johnson", new DateTime(2016, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1800 });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeID", "DepartmentId", "FirstName", "LastName", "Recruitment", "Salary" },
                values: new object[] { 3, 2, "Jane", "Smith", new DateTime(2016, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1700 });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}

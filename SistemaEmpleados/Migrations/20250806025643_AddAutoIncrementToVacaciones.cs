using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEmpleados.Migrations
{
    /// <inheritdoc />
    public partial class AddAutoIncrementToVacaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    CargoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cargos__B4E665ED471A38DD", x => x.CargoID);
                });

            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    DepartamentoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Departam__66BB0E1E8D533A1D", x => x.DepartamentoID);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    EmpleadoID = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartamentoID = table.Column<int>(type: "int", nullable: false),
                    CargoID = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Empleado__958BE6F08CE0019C", x => x.EmpleadoID);
                    table.ForeignKey(
                        name: "FK__Empleados__Cargo__3D5E1FD2",
                        column: x => x.CargoID,
                        principalTable: "Cargos",
                        principalColumn: "CargoID");
                    table.ForeignKey(
                        name: "FK__Empleados__Depar__3C69FB99",
                        column: x => x.DepartamentoID,
                        principalTable: "Departamentos",
                        principalColumn: "DepartamentoID");
                });

            migrationBuilder.CreateTable(
                name: "VacacionesDisponibles",
                columns: table => new
                {
                    VacacionesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoID = table.Column<int>(type: "int", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    DiasAsignados = table.Column<int>(type: "int", nullable: false),
                    DiasTomados = table.Column<int>(type: "int", nullable: false),
                    DiasRestantes = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([DiasAsignados]-[DiasTomados])", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vacacion__DC3149A01E32B20C", x => x.VacacionesID);
                    table.ForeignKey(
                        name: "FK__Vacacione__Emple__440B1D61",
                        column: x => x.EmpleadoID,
                        principalTable: "Empleados",
                        principalColumn: "EmpleadoID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_CargoID",
                table: "Empleados",
                column: "CargoID");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_DepartamentoID",
                table: "Empleados",
                column: "DepartamentoID");

            migrationBuilder.CreateIndex(
                name: "IX_VacacionesDisponibles_EmpleadoID",
                table: "VacacionesDisponibles",
                column: "EmpleadoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VacacionesDisponibles");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Cargos");

            migrationBuilder.DropTable(
                name: "Departamentos");
        }
    }
}

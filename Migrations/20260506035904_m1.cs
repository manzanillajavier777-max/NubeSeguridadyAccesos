using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Seguridad.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dispositivo",
                columns: table => new
                {
                    Id_Dispositivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoArea = table.Column<string>(type: "text", nullable: false),
                    NombreDispositivo = table.Column<string>(type: "text", nullable: false),
                    NumeroSerie = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivo", x => x.Id_Dispositivo);
                });

            migrationBuilder.CreateTable(
                name: "Equipamiento",
                columns: table => new
                {
                    Id_Equipamiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre_Equipo = table.Column<string>(type: "text", nullable: false),
                    CodigoEquipamiento = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipamiento", x => x.Id_Equipamiento);
                });

            migrationBuilder.CreateTable(
                name: "Horario",
                columns: table => new
                {
                    Id_Horario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoHorario = table.Column<string>(type: "text", nullable: false),
                    HoraInicio = table.Column<string>(type: "text", nullable: false),
                    HoraFin = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horario", x => x.Id_Horario);
                });

            migrationBuilder.CreateTable(
                name: "IdentidadAcceso",
                columns: table => new
                {
                    Id_Identidad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoPersona = table.Column<string>(type: "text", nullable: false),
                    TipoPersona = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentidadAcceso", x => x.Id_Identidad);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id_Rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoRol = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id_Rol);
                });

            migrationBuilder.CreateTable(
                name: "AccesoRegistros",
                columns: table => new
                {
                    id_Registros = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentidadId = table.Column<int>(type: "integer", nullable: false),
                    DispositivoId = table.Column<int>(type: "integer", nullable: false),
                    CodigoArea = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateOnly>(type: "date", nullable: false),
                    HorarioRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Id_Identidad = table.Column<int>(type: "integer", nullable: false),
                    Id_Dispositivo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccesoRegistros", x => x.id_Registros);
                    table.ForeignKey(
                        name: "FK_AccesoRegistros_Dispositivo_Id_Dispositivo",
                        column: x => x.Id_Dispositivo,
                        principalTable: "Dispositivo",
                        principalColumn: "Id_Dispositivo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccesoRegistros_IdentidadAcceso_Id_Identidad",
                        column: x => x.Id_Identidad,
                        principalTable: "IdentidadAcceso",
                        principalColumn: "Id_Identidad",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsignacionEquipamientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    identidadId = table.Column<int>(type: "integer", nullable: false),
                    equipamientoId = table.Column<int>(type: "integer", nullable: false),
                    fechaEntrega = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Id_Identidad = table.Column<int>(type: "integer", nullable: false),
                    Id_Equipamiento = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionEquipamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsignacionEquipamientos_Equipamiento_Id_Equipamiento",
                        column: x => x.Id_Equipamiento,
                        principalTable: "Equipamiento",
                        principalColumn: "Id_Equipamiento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignacionEquipamientos_IdentidadAcceso_Id_Identidad",
                        column: x => x.Id_Identidad,
                        principalTable: "IdentidadAcceso",
                        principalColumn: "Id_Identidad",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DatosBiometricos",
                columns: table => new
                {
                    Id_DatosBio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoDato = table.Column<string>(type: "text", nullable: false),
                    TipoDatoBiometrico = table.Column<string>(type: "text", nullable: false),
                    DatoHuella = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    IdentidadId = table.Column<int>(type: "integer", nullable: false),
                    Id_Identidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatosBiometricos", x => x.Id_DatosBio);
                    table.ForeignKey(
                        name: "FK_DatosBiometricos_IdentidadAcceso_Id_Identidad",
                        column: x => x.Id_Identidad,
                        principalTable: "IdentidadAcceso",
                        principalColumn: "Id_Identidad",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reporte",
                columns: table => new
                {
                    Id_Reporte = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigoReporte = table.Column<string>(type: "text", nullable: false),
                    TipoReporte = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    FechaReporte = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    IdentidadId = table.Column<int>(type: "integer", nullable: false),
                    Id_Identidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporte", x => x.Id_Reporte);
                    table.ForeignKey(
                        name: "FK_Reporte_IdentidadAcceso_Id_Identidad",
                        column: x => x.Id_Identidad,
                        principalTable: "IdentidadAcceso",
                        principalColumn: "Id_Identidad",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolHorario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigoArea = table.Column<string>(type: "text", nullable: false),
                    rolId = table.Column<int>(type: "integer", nullable: false),
                    HoraioId = table.Column<int>(type: "integer", nullable: false),
                    Id_Horario = table.Column<int>(type: "integer", nullable: false),
                    Id_Rol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolHorario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolHorario_Horario_Id_Horario",
                        column: x => x.Id_Horario,
                        principalTable: "Horario",
                        principalColumn: "Id_Horario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolHorario_Rol_Id_Rol",
                        column: x => x.Id_Rol,
                        principalTable: "Rol",
                        principalColumn: "Id_Rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolIdentidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    IdentidadId = table.Column<int>(type: "integer", nullable: false),
                    Id_Identidad = table.Column<int>(type: "integer", nullable: false),
                    Id_Rol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolIdentidad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolIdentidad_IdentidadAcceso_Id_Identidad",
                        column: x => x.Id_Identidad,
                        principalTable: "IdentidadAcceso",
                        principalColumn: "Id_Identidad",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolIdentidad_Rol_Id_Rol",
                        column: x => x.Id_Rol,
                        principalTable: "Rol",
                        principalColumn: "Id_Rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccesoRegistros_Id_Dispositivo",
                table: "AccesoRegistros",
                column: "Id_Dispositivo");

            migrationBuilder.CreateIndex(
                name: "IX_AccesoRegistros_Id_Identidad",
                table: "AccesoRegistros",
                column: "Id_Identidad");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionEquipamientos_Id_Equipamiento",
                table: "AsignacionEquipamientos",
                column: "Id_Equipamiento");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionEquipamientos_Id_Identidad",
                table: "AsignacionEquipamientos",
                column: "Id_Identidad");

            migrationBuilder.CreateIndex(
                name: "IX_DatosBiometricos_Id_Identidad",
                table: "DatosBiometricos",
                column: "Id_Identidad");

            migrationBuilder.CreateIndex(
                name: "IX_Reporte_Id_Identidad",
                table: "Reporte",
                column: "Id_Identidad");

            migrationBuilder.CreateIndex(
                name: "IX_RolHorario_Id_Horario",
                table: "RolHorario",
                column: "Id_Horario");

            migrationBuilder.CreateIndex(
                name: "IX_RolHorario_Id_Rol",
                table: "RolHorario",
                column: "Id_Rol");

            migrationBuilder.CreateIndex(
                name: "IX_RolIdentidad_Id_Identidad",
                table: "RolIdentidad",
                column: "Id_Identidad");

            migrationBuilder.CreateIndex(
                name: "IX_RolIdentidad_Id_Rol",
                table: "RolIdentidad",
                column: "Id_Rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccesoRegistros");

            migrationBuilder.DropTable(
                name: "AsignacionEquipamientos");

            migrationBuilder.DropTable(
                name: "DatosBiometricos");

            migrationBuilder.DropTable(
                name: "Reporte");

            migrationBuilder.DropTable(
                name: "RolHorario");

            migrationBuilder.DropTable(
                name: "RolIdentidad");

            migrationBuilder.DropTable(
                name: "Dispositivo");

            migrationBuilder.DropTable(
                name: "Equipamiento");

            migrationBuilder.DropTable(
                name: "Horario");

            migrationBuilder.DropTable(
                name: "IdentidadAcceso");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}

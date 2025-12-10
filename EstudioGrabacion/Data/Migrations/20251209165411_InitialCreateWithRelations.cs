using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstudioGrabacion.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sesiones_AspNetUsers_UsuarioId",
                table: "Sesiones");

            migrationBuilder.DropTable(
                name: "PaqueteServicios");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Sesiones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pendiente",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Servicios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Servicios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Ingenieros",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Ingenieros",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Ingenieros",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Ingenieros",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServicioPaquetes",
                columns: table => new
                {
                    PaqueteId = table.Column<int>(type: "int", nullable: false),
                    ServicioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicioPaquetes", x => new { x.PaqueteId, x.ServicioId });
                    table.ForeignKey(
                        name: "FK_ServicioPaquetes_Paquetes_PaqueteId",
                        column: x => x.PaqueteId,
                        principalTable: "Paquetes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicioPaquetes_Servicios_ServicioId",
                        column: x => x.ServicioId,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_Activo",
                table: "Servicios",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Ingenieros_Disponible",
                table: "Ingenieros",
                column: "Disponible");

            migrationBuilder.CreateIndex(
                name: "IX_ServicioPaquetes_ServicioId",
                table: "ServicioPaquetes",
                column: "ServicioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sesiones_AspNetUsers_UsuarioId",
                table: "Sesiones",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sesiones_AspNetUsers_UsuarioId",
                table: "Sesiones");

            migrationBuilder.DropTable(
                name: "ServicioPaquetes");

            migrationBuilder.DropIndex(
                name: "IX_Servicios_Activo",
                table: "Servicios");

            migrationBuilder.DropIndex(
                name: "IX_Ingenieros_Disponible",
                table: "Ingenieros");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Servicios");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Servicios");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Ingenieros");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Ingenieros");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Sesiones",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Pendiente");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Ingenieros",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Ingenieros",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PaqueteServicios",
                columns: table => new
                {
                    PaquetesId = table.Column<int>(type: "int", nullable: false),
                    ServiciosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaqueteServicios", x => new { x.PaquetesId, x.ServiciosId });
                    table.ForeignKey(
                        name: "FK_PaqueteServicios_Paquetes_PaquetesId",
                        column: x => x.PaquetesId,
                        principalTable: "Paquetes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaqueteServicios_Servicios_ServiciosId",
                        column: x => x.ServiciosId,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaqueteServicios_ServiciosId",
                table: "PaqueteServicios",
                column: "ServiciosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sesiones_AspNetUsers_UsuarioId",
                table: "Sesiones",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

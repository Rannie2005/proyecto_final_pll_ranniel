using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstudioGrabacion.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposClienteSesion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sesiones_AspNetUsers_UsuarioId",
                table: "Sesiones");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Sesiones",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "DireccionCliente",
                table: "Sesiones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailCliente",
                table: "Sesiones",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreCliente",
                table: "Sesiones",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoCliente",
                table: "Sesiones",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sesiones_AspNetUsers_UsuarioId",
                table: "Sesiones",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sesiones_AspNetUsers_UsuarioId",
                table: "Sesiones");

            migrationBuilder.DropColumn(
                name: "DireccionCliente",
                table: "Sesiones");

            migrationBuilder.DropColumn(
                name: "EmailCliente",
                table: "Sesiones");

            migrationBuilder.DropColumn(
                name: "NombreCliente",
                table: "Sesiones");

            migrationBuilder.DropColumn(
                name: "TelefonoCliente",
                table: "Sesiones");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Sesiones",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sesiones_AspNetUsers_UsuarioId",
                table: "Sesiones",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

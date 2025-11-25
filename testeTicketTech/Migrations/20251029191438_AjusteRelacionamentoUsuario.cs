using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testeTicketTech.Migrations
{
    public partial class AjusteRelacionamentoUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TokenRedefinicao",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Chamados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioModelId",
                table: "Chamados",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_UsuarioId",
                table: "Chamados",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_UsuarioModelId",
                table: "Chamados",
                column: "UsuarioModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Usuarios_UsuarioId",
                table: "Chamados",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Usuarios_UsuarioModelId",
                table: "Chamados",
                column: "UsuarioModelId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Usuarios_UsuarioId",
                table: "Chamados");

            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Usuarios_UsuarioModelId",
                table: "Chamados");

            migrationBuilder.DropIndex(
                name: "IX_Chamados_UsuarioId",
                table: "Chamados");

            migrationBuilder.DropIndex(
                name: "IX_Chamados_UsuarioModelId",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "UsuarioModelId",
                table: "Chamados");

            migrationBuilder.AlterColumn<string>(
                name: "TokenRedefinicao",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Chamados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

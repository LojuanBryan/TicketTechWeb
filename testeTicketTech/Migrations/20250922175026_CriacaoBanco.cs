using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testeTicketTech.Migrations
{
    public partial class CriacaoBanco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chamados",
                columns: table => new
                {
                    ChamadoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Dispositivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sintomas = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    QuandoOcorreu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OndeOcorreu = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DescricaoDetalhada = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chamados", x => x.ChamadoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chamados");
        }
    }
}

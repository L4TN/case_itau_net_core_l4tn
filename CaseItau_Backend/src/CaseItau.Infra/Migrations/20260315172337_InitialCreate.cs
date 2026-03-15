using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CaseItau.Infra.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Feature_Flag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ds_Chave = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Fl_Habilitado = table.Column<bool>(type: "bit", nullable: false),
                    Ds_Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Json_Config = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dt_Criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dt_Atualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Feature_Flag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Tipo_Fundo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nm_Tipo_Fundo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Tipo_Fundo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Fundo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cd_Fundo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nm_Fundo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nr_Cnpj = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Id_Tipo_Fundo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Fundo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_Fundo_Tb_Tipo_Fundo_Id_Tipo_Fundo",
                        column: x => x.Id_Tipo_Fundo,
                        principalTable: "Tb_Tipo_Fundo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Movimentacao_Fundo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Fundo = table.Column<int>(type: "int", nullable: false),
                    Dt_Movimentacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Vlr_Movimentacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Movimentacao_Fundo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_Movimentacao_Fundo_Tb_Fundo_Id_Fundo",
                        column: x => x.Id_Fundo,
                        principalTable: "Tb_Fundo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Posicao_Fundo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Fundo = table.Column<int>(type: "int", nullable: false),
                    Dt_Posicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Vlr_Patrimonio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Row_Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Posicao_Fundo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_Posicao_Fundo_Tb_Fundo_Id_Fundo",
                        column: x => x.Id_Fundo,
                        principalTable: "Tb_Fundo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tb_Feature_Flag",
                columns: new[] { "Id", "Ds_Chave", "Dt_Atualizacao", "Dt_Criacao", "Ds_Descricao", "Fl_Habilitado", "Json_Config" },
                values: new object[] { 1, "cache_redis", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Habilita/desabilita o uso de cache Redis nas consultas de movimentações e posições.", true, "{\"ExpirationInSeconds\": 60}" });

            migrationBuilder.InsertData(
                table: "Tb_Tipo_Fundo",
                columns: new[] { "Id", "Nm_Tipo_Fundo" },
                values: new object[,]
                {
                    { 1, "RENDA FIXA" },
                    { 2, "ACOES" },
                    { 3, "MULTI MERCADO" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Feature_Flag_Ds_Chave",
                table: "Tb_Feature_Flag",
                column: "Ds_Chave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Fundo_Cd_Fundo",
                table: "Tb_Fundo",
                column: "Cd_Fundo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Fundo_Id_Tipo_Fundo",
                table: "Tb_Fundo",
                column: "Id_Tipo_Fundo");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Fundo_Nr_Cnpj",
                table: "Tb_Fundo",
                column: "Nr_Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Movimentacao_Fundo_Id_Fundo",
                table: "Tb_Movimentacao_Fundo",
                column: "Id_Fundo");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Posicao_Fundo_Id_Fundo",
                table: "Tb_Posicao_Fundo",
                column: "Id_Fundo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Feature_Flag");

            migrationBuilder.DropTable(
                name: "Tb_Movimentacao_Fundo");

            migrationBuilder.DropTable(
                name: "Tb_Posicao_Fundo");

            migrationBuilder.DropTable(
                name: "Tb_Fundo");

            migrationBuilder.DropTable(
                name: "Tb_Tipo_Fundo");
        }
    }
}

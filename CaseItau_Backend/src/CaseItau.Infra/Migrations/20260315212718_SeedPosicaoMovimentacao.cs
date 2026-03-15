using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CaseItau.Infra.Migrations
{
    /// <inheritdoc />
    public partial class SeedPosicaoMovimentacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tb_Movimentacao_Fundo",
                columns: new[] { "Id", "Dt_Movimentacao", "Id_Fundo", "Vlr_Movimentacao" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 50000.00m },
                    { 2, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, -15000.00m },
                    { 3, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 30000.00m },
                    { 4, new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, -8500.00m },
                    { 5, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 25000.00m },
                    { 6, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 100000.00m },
                    { 7, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, -45000.00m },
                    { 8, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 75000.00m },
                    { 9, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, -20000.00m },
                    { 10, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 60000.00m },
                    { 11, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 25000.00m },
                    { 12, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, -10000.00m },
                    { 13, new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 40000.00m },
                    { 14, new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, -55000.00m },
                    { 15, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 35000.00m },
                    { 16, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 80000.00m },
                    { 17, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, -60000.00m },
                    { 18, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 120000.00m },
                    { 19, new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, -35000.00m },
                    { 20, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 45000.00m }
                });

            migrationBuilder.InsertData(
                table: "Tb_Posicao_Fundo",
                columns: new[] { "Id", "Dt_Posicao", "Id_Fundo", "Vlr_Patrimonio" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1250000.00m },
                    { 2, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1275430.50m },
                    { 3, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1310200.75m },
                    { 4, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1298750.30m },
                    { 5, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1345600.00m },
                    { 6, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3500000.00m },
                    { 7, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3528750.25m },
                    { 8, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3560100.80m },
                    { 9, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3545320.10m },
                    { 10, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3612400.00m },
                    { 11, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 780000.00m },
                    { 12, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 795230.40m },
                    { 13, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 810500.90m },
                    { 14, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 768200.60m },
                    { 15, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 825750.00m },
                    { 16, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2100000.00m },
                    { 17, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2045300.80m },
                    { 18, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2180750.25m },
                    { 19, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2230100.50m },
                    { 20, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2150400.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Tb_Movimentacao_Fundo",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Tb_Posicao_Fundo",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}

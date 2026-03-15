using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infra.Data.Dbo;

public class TbPosicaoFundoMap : IEntityTypeConfiguration<TbPosicaoFundo>
{
    public void Configure(EntityTypeBuilder<TbPosicaoFundo> builder)
    {
        builder.ToTable("Tb_Posicao_Fundo");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id");

        builder.Property(p => p.FundoId)
            .HasColumnName("Id_Fundo")
            .IsRequired();

        builder.Property(p => p.DataPosicao)
            .HasColumnName("Dt_Posicao")
            .IsRequired();

        builder.Property(p => p.VlrPatrimonio)
            .HasColumnName("Vlr_Patrimonio")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.RowVersion)
            .HasColumnName("Row_Version")
            .IsRowVersion();

        builder.HasOne(p => p.Fundo)
            .WithMany(f => f.Posicoes)
            .HasForeignKey(p => p.FundoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new { Id = 1, FundoId = 1, DataPosicao = new DateTime(2026, 1, 2), VlrPatrimonio = 1_250_000.00m },
            new { Id = 2, FundoId = 1, DataPosicao = new DateTime(2026, 1, 15), VlrPatrimonio = 1_275_430.50m },
            new { Id = 3, FundoId = 1, DataPosicao = new DateTime(2026, 2, 3), VlrPatrimonio = 1_310_200.75m },
            new { Id = 4, FundoId = 1, DataPosicao = new DateTime(2026, 2, 17), VlrPatrimonio = 1_298_750.30m },
            new { Id = 5, FundoId = 1, DataPosicao = new DateTime(2026, 3, 3), VlrPatrimonio = 1_345_600.00m },
            new { Id = 6, FundoId = 2, DataPosicao = new DateTime(2026, 1, 2), VlrPatrimonio = 3_500_000.00m },
            new { Id = 7, FundoId = 2, DataPosicao = new DateTime(2026, 1, 15), VlrPatrimonio = 3_528_750.25m },
            new { Id = 8, FundoId = 2, DataPosicao = new DateTime(2026, 2, 3), VlrPatrimonio = 3_560_100.80m },
            new { Id = 9, FundoId = 2, DataPosicao = new DateTime(2026, 2, 17), VlrPatrimonio = 3_545_320.10m },
            new { Id = 10, FundoId = 2, DataPosicao = new DateTime(2026, 3, 3), VlrPatrimonio = 3_612_400.00m },
            new { Id = 11, FundoId = 3, DataPosicao = new DateTime(2026, 1, 2), VlrPatrimonio = 780_000.00m },
            new { Id = 12, FundoId = 3, DataPosicao = new DateTime(2026, 1, 15), VlrPatrimonio = 795_230.40m },
            new { Id = 13, FundoId = 3, DataPosicao = new DateTime(2026, 2, 3), VlrPatrimonio = 810_500.90m },
            new { Id = 14, FundoId = 3, DataPosicao = new DateTime(2026, 2, 17), VlrPatrimonio = 768_200.60m },
            new { Id = 15, FundoId = 3, DataPosicao = new DateTime(2026, 3, 3), VlrPatrimonio = 825_750.00m },
            new { Id = 16, FundoId = 4, DataPosicao = new DateTime(2026, 1, 2), VlrPatrimonio = 2_100_000.00m },
            new { Id = 17, FundoId = 4, DataPosicao = new DateTime(2026, 1, 15), VlrPatrimonio = 2_045_300.80m },
            new { Id = 18, FundoId = 4, DataPosicao = new DateTime(2026, 2, 3), VlrPatrimonio = 2_180_750.25m },
            new { Id = 19, FundoId = 4, DataPosicao = new DateTime(2026, 2, 17), VlrPatrimonio = 2_230_100.50m },
            new { Id = 20, FundoId = 4, DataPosicao = new DateTime(2026, 3, 3), VlrPatrimonio = 2_150_400.00m }
        );
    }
}

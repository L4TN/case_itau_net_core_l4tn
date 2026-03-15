using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infra.Data.Dbo;

public class TbMovimentacaoFundoMap : IEntityTypeConfiguration<TbMovimentacaoFundo>
{
    public void Configure(EntityTypeBuilder<TbMovimentacaoFundo> builder)
    {
        builder.ToTable("Tb_Movimentacao_Fundo");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("Id");

        builder.Property(m => m.FundoId)
            .HasColumnName("Id_Fundo")
            .IsRequired();

        builder.Property(m => m.DataMovimentacao)
            .HasColumnName("Dt_Movimentacao")
            .IsRequired();

        builder.Property(m => m.VlrMovimentacao)
            .HasColumnName("Vlr_Movimentacao")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(m => m.Fundo)
            .WithMany(f => f.Movimentacoes)
            .HasForeignKey(m => m.FundoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new { Id = 1, FundoId = 1, DataMovimentacao = new DateTime(2026, 1, 5), VlrMovimentacao = 50_000.00m },
            new { Id = 2, FundoId = 1, DataMovimentacao = new DateTime(2026, 1, 20), VlrMovimentacao = -15_000.00m },
            new { Id = 3, FundoId = 1, DataMovimentacao = new DateTime(2026, 2, 10), VlrMovimentacao = 30_000.00m },
            new { Id = 4, FundoId = 1, DataMovimentacao = new DateTime(2026, 2, 25), VlrMovimentacao = -8_500.00m },
            new { Id = 5, FundoId = 1, DataMovimentacao = new DateTime(2026, 3, 5), VlrMovimentacao = 25_000.00m },
            new { Id = 6, FundoId = 2, DataMovimentacao = new DateTime(2026, 1, 3), VlrMovimentacao = 100_000.00m },
            new { Id = 7, FundoId = 2, DataMovimentacao = new DateTime(2026, 1, 18), VlrMovimentacao = -45_000.00m },
            new { Id = 8, FundoId = 2, DataMovimentacao = new DateTime(2026, 2, 5), VlrMovimentacao = 75_000.00m },
            new { Id = 9, FundoId = 2, DataMovimentacao = new DateTime(2026, 2, 20), VlrMovimentacao = -20_000.00m },
            new { Id = 10, FundoId = 2, DataMovimentacao = new DateTime(2026, 3, 7), VlrMovimentacao = 60_000.00m },
            new { Id = 11, FundoId = 3, DataMovimentacao = new DateTime(2026, 1, 8), VlrMovimentacao = 25_000.00m },
            new { Id = 12, FundoId = 3, DataMovimentacao = new DateTime(2026, 1, 22), VlrMovimentacao = -10_000.00m },
            new { Id = 13, FundoId = 3, DataMovimentacao = new DateTime(2026, 2, 12), VlrMovimentacao = 40_000.00m },
            new { Id = 14, FundoId = 3, DataMovimentacao = new DateTime(2026, 2, 28), VlrMovimentacao = -55_000.00m },
            new { Id = 15, FundoId = 3, DataMovimentacao = new DateTime(2026, 3, 10), VlrMovimentacao = 35_000.00m },
            new { Id = 16, FundoId = 4, DataMovimentacao = new DateTime(2026, 1, 6), VlrMovimentacao = 80_000.00m },
            new { Id = 17, FundoId = 4, DataMovimentacao = new DateTime(2026, 1, 25), VlrMovimentacao = -60_000.00m },
            new { Id = 18, FundoId = 4, DataMovimentacao = new DateTime(2026, 2, 8), VlrMovimentacao = 120_000.00m },
            new { Id = 19, FundoId = 4, DataMovimentacao = new DateTime(2026, 2, 22), VlrMovimentacao = -35_000.00m },
            new { Id = 20, FundoId = 4, DataMovimentacao = new DateTime(2026, 3, 1), VlrMovimentacao = 45_000.00m }
        );
    }
}

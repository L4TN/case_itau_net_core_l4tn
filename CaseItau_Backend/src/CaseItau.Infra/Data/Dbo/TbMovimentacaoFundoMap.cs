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
    }
}

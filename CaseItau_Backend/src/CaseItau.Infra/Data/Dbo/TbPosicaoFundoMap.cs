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
    }
}

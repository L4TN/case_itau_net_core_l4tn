using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infra.Data.Dbo;

public class TbFundoMap : IEntityTypeConfiguration<TbFundo>
{
    public void Configure(EntityTypeBuilder<TbFundo> builder)
    {
        builder.ToTable("Tb_Fundo");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasColumnName("Id");

        builder.Property(f => f.Codigo)
            .HasColumnName("Cd_Fundo")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(f => f.Codigo).IsUnique();

        builder.Property(f => f.Nome)
            .HasColumnName("Nm_Fundo")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(f => f.Cnpj)
            .HasColumnName("Nr_Cnpj")
            .HasMaxLength(14)
            .IsRequired();

        builder.HasIndex(f => f.Cnpj).IsUnique();

        builder.Property(f => f.TipoFundoId)
            .HasColumnName("Id_Tipo_Fundo")
            .IsRequired();

        builder.HasOne(f => f.TipoFundo)
            .WithMany(t => t.Fundos)
            .HasForeignKey(f => f.TipoFundoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(f => f.Posicoes)
            .WithOne(p => p.Fundo)
            .HasForeignKey(p => p.FundoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

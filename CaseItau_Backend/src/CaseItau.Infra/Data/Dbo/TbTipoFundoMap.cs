using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infra.Data.Dbo;

public class TbTipoFundoMap : IEntityTypeConfiguration<TbTipoFundo>
{
    public void Configure(EntityTypeBuilder<TbTipoFundo> builder)
    {
        builder.ToTable("Tb_Tipo_Fundo");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("Id");

        builder.Property(t => t.Nome)
            .HasColumnName("Nm_Tipo_Fundo")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasMany(t => t.Fundos)
            .WithOne(f => f.TipoFundo)
            .HasForeignKey(f => f.TipoFundoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new TbTipoFundo { Id = 1, Nome = "RENDA FIXA" },
            new TbTipoFundo { Id = 2, Nome = "ACOES" },
            new TbTipoFundo { Id = 3, Nome = "MULTI MERCADO" }
        );
    }
}

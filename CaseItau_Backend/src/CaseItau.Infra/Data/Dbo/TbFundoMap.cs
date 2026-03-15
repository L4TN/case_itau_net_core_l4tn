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

        builder.HasData(
            new { Id = 1, Codigo = "ITAU1", Nome = "Itaú Privilège RF DI", Cnpj = "26199519000134", TipoFundoId = 1 },
            new { Id = 2, Codigo = "ITAU2", Nome = "Itaú Index Simples Selic RF FICFI", Cnpj = "36347721000110", TipoFundoId = 1 },
            new { Id = 3, Codigo = "ITAU3", Nome = "Itaú Janeiro Multimercado FICFI", Cnpj = "52116227000109", TipoFundoId = 3 },
            new { Id = 4, Codigo = "ITAU4", Nome = "Itaú Ações Momento 30 FICFI", Cnpj = "16718302000130", TipoFundoId = 2 }
        );
    }
}

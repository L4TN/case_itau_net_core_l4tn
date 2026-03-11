using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infra.Data.Dbo;

public class TbFeatureFlagMap : IEntityTypeConfiguration<TbFeatureFlag>
{
    public void Configure(EntityTypeBuilder<TbFeatureFlag> builder)
    {
        builder.ToTable("Tb_Feature_Flag");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasColumnName("Id")
            .UseIdentityColumn();

        builder.Property(f => f.Chave)
            .HasColumnName("Ds_Chave")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(f => f.Chave).IsUnique();

        builder.Property(f => f.Habilitado)
            .HasColumnName("Fl_Habilitado")
            .IsRequired();

        builder.Property(f => f.Descricao)
            .HasColumnName("Ds_Descricao")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(f => f.JsonConfig)
            .HasColumnName("Json_Config")
            .HasColumnType("nvarchar(max)");

        builder.Property(f => f.DataCriacao)
            .HasColumnName("Dt_Criacao")
            .IsRequired();

        builder.Property(f => f.DataAtualizacao)
            .HasColumnName("Dt_Atualizacao");

        builder.HasData(new TbFeatureFlag
        {
            Id = 1,
            Chave = "cache_redis",
            Habilitado = true,
            Descricao = "Habilita/desabilita o uso de cache Redis nas consultas de movimentações e posições.",
            JsonConfig = "{\"ExpirationInSeconds\": 60}",
            DataCriacao = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}

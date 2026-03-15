using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infra.Data;

public class DboContext : BaseDbContext
{
    public DbSet<TbFundo> Fundos { get; set; }
    public DbSet<TbTipoFundo> TiposFundo { get; set; }
    public DbSet<TbPosicaoFundo> PosicoesFundo { get; set; }
    public DbSet<TbMovimentacaoFundo> MovimentacoesFundo { get; set; }
    public DbSet<TbFeatureFlag> FeatureFlags { get; set; }

    public DboContext(DbContextOptions<DboContext> options) : base(options) { }
}

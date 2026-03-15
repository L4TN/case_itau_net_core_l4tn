using CaseItau.Domain.Interfaces;

namespace CaseItau.Infra.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DboContext _context;

    public IFundoRepository Fundos { get; }
    public ITipoFundoRepository TiposFundo { get; }
    public IMovimentacaoRepository Movimentacoes { get; }
    public IPosicaoFundoRepository PosicoesFundo { get; }
    public IFeatureFlagRepository FeatureFlags { get; }

    public UnitOfWork(
        DboContext context,
        IFundoRepository fundos,
        ITipoFundoRepository tiposFundo,
        IMovimentacaoRepository movimentacoes,
        IPosicaoFundoRepository posicoesFundo,
        IFeatureFlagRepository featureFlags)
    {
        _context = context;
        Fundos = fundos;
        TiposFundo = tiposFundo;
        Movimentacoes = movimentacoes;
        PosicoesFundo = posicoesFundo;
        FeatureFlags = featureFlags;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}

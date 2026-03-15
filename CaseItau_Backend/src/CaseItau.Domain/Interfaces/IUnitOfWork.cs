namespace CaseItau.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IFundoRepository Fundos { get; }
    ITipoFundoRepository TiposFundo { get; }
    IMovimentacaoRepository Movimentacoes { get; }
    IPosicaoFundoRepository PosicoesFundo { get; }
    IFeatureFlagRepository FeatureFlags { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}

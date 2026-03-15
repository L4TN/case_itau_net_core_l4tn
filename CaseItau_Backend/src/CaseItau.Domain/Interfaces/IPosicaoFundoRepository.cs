using CaseItau.Domain.Entities;

namespace CaseItau.Domain.Interfaces;

public interface IPosicaoFundoRepository
{
    Task<IEnumerable<TbPosicaoFundo>> GetByFundoIdAsync(int fundoId, CancellationToken cancellationToken = default);
    Task<TbPosicaoFundo?> GetByFundoIdAndDateAsync(int fundoId, DateTime date, CancellationToken cancellationToken = default);
    Task<TbPosicaoFundo?> GetLatestByFundoIdAsync(int fundoId, CancellationToken cancellationToken = default);
    Task AddAsync(TbPosicaoFundo posicao, CancellationToken cancellationToken = default);
    Task UpdateAsync(TbPosicaoFundo posicao, CancellationToken cancellationToken = default);
}

using CaseItau.Domain.Entities;

namespace CaseItau.Domain.Interfaces;

public interface IMovimentacaoRepository
{
    Task AddAsync(TbMovimentacaoFundo movimentacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<TbMovimentacaoFundo>> GetByFundoIdAsync(int fundoId, CancellationToken cancellationToken = default);
}

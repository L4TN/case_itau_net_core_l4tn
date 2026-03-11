using CaseItau.Domain.Entities;

namespace CaseItau.Domain.Interfaces;

public interface ITipoFundoRepository
{
    Task<IEnumerable<TbTipoFundo>> GetAllTpFundsAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsFundByIdAsync(int id, CancellationToken cancellationToken = default);
}

using CaseItau.Domain.Entities;

namespace CaseItau.Domain.Interfaces;

public interface IFundoRepository
{
    Task<IEnumerable<TbFundo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IEnumerable<TbFundo> Items, int TotalCount)> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<TbFundo?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task AddAsync(TbFundo fundo, CancellationToken cancellationToken = default);
    Task UpdateAsync(TbFundo fundo, CancellationToken cancellationToken = default);
    Task DeleteAsync(string codigo, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCnpjAsync(string cnpj, string? codigoExcluir = null, CancellationToken cancellationToken = default);
}

using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;
using CaseItau.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infra.Repositories;

public class TipoFundoRepository : ITipoFundoRepository
{
    private readonly DboContext _context;

    public TipoFundoRepository(DboContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TbTipoFundo>> GetAllTpFundsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TiposFundo
            .AsNoTracking()
            .OrderBy(t => t.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsFundByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TiposFundo.AnyAsync(t => t.Id == id, cancellationToken);
    }
}

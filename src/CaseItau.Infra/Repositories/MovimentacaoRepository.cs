using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;
using CaseItau.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infra.Repositories;

public class MovimentacaoRepository : IMovimentacaoRepository
{
    private readonly DboContext _context;

    public MovimentacaoRepository(DboContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TbMovimentacaoFundo movimentacao, CancellationToken cancellationToken = default)
    {
        await _context.MovimentacoesFundo.AddAsync(movimentacao, cancellationToken);
    }

    public async Task<IEnumerable<TbMovimentacaoFundo>> GetByFundoIdAsync(int fundoId, CancellationToken cancellationToken = default)
    {
        return await _context.MovimentacoesFundo
            .AsNoTracking()
            .Where(m => m.FundoId == fundoId)
            .OrderBy(m => m.DataMovimentacao)
            .ToListAsync(cancellationToken);
    }
}

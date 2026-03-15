using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;
using CaseItau.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infra.Repositories;

public class PosicaoFundoRepository : IPosicaoFundoRepository
{
    private readonly DboContext _context;

    public PosicaoFundoRepository(DboContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TbPosicaoFundo>> GetByFundoIdAsync(int fundoId, CancellationToken cancellationToken = default)
    {
        return await _context.PosicoesFundo
            .AsNoTracking()
            .Where(p => p.FundoId == fundoId)
            .OrderBy(p => p.DataPosicao)
            .ToListAsync(cancellationToken);
    }

    public async Task<TbPosicaoFundo?> GetByFundoIdAndDateAsync(int fundoId, DateTime date, CancellationToken cancellationToken = default)
    {
        return await _context.PosicoesFundo
            .FirstOrDefaultAsync(p => p.FundoId == fundoId && p.DataPosicao.Date == date.Date, cancellationToken);
    }

    public async Task<TbPosicaoFundo?> GetLatestByFundoIdAsync(int fundoId, CancellationToken cancellationToken = default)
    {
        return await _context.PosicoesFundo
            .AsNoTracking()
            .Where(p => p.FundoId == fundoId)
            .OrderByDescending(p => p.DataPosicao)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(TbPosicaoFundo posicao, CancellationToken cancellationToken = default)
    {
        await _context.PosicoesFundo.AddAsync(posicao, cancellationToken);
    }

    public Task UpdateAsync(TbPosicaoFundo posicao, CancellationToken cancellationToken = default)
    {
        _context.PosicoesFundo.Update(posicao);
        return Task.CompletedTask;
    }
}

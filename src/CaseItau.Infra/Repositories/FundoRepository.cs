using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;
using CaseItau.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infra.Repositories;

public class FundoRepository : IFundoRepository
{
    private readonly DboContext _context;

    public FundoRepository(DboContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TbFundo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Fundos
            .AsNoTracking()
            .Include(f => f.TipoFundo)
            .OrderBy(f => f.Nome)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<TbFundo> Items, int TotalCount)> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Fundos
            .AsNoTracking()
            .Include(f => f.TipoFundo)
            .OrderBy(f => f.Nome);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<TbFundo?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _context.Fundos
            .AsNoTracking()
            .Include(f => f.TipoFundo)
            .FirstOrDefaultAsync(f => f.Codigo == codigo, cancellationToken);
    }

    public async Task AddAsync(TbFundo fundo, CancellationToken cancellationToken = default)
    {
        await _context.Fundos.AddAsync(fundo, cancellationToken);
    }

    public Task UpdateAsync(TbFundo fundo, CancellationToken cancellationToken = default)
    {
        _context.Fundos.Update(fundo);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string codigo, CancellationToken cancellationToken = default)
    {
        var fundo = await _context.Fundos
            .Include(f => f.Posicoes)
            .Include(f => f.Movimentacoes)
            .FirstOrDefaultAsync(f => f.Codigo == codigo, cancellationToken);
        if (fundo != null)
        {
            _context.PosicoesFundo.RemoveRange(fundo.Posicoes);
            _context.MovimentacoesFundo.RemoveRange(fundo.Movimentacoes);
            _context.Fundos.Remove(fundo);
        }
    }

    public async Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        return await _context.Fundos.AnyAsync(f => f.Codigo == codigo, cancellationToken);
    }

    public async Task<bool> ExistsByCnpjAsync(string cnpj, string? codigoExcluir = null, CancellationToken cancellationToken = default)
    {
        return await _context.Fundos.AnyAsync(
            f => f.Cnpj == cnpj && (codigoExcluir == null || f.Codigo != codigoExcluir),
            cancellationToken);
    }
}

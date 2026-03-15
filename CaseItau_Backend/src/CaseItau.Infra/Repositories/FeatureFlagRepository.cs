using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;
using CaseItau.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infra.Repositories;

public class FeatureFlagRepository : IFeatureFlagRepository
{
    private readonly DboContext _context;

    public FeatureFlagRepository(DboContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TbFeatureFlag>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.FeatureFlags
            .AsNoTracking()
            .OrderBy(f => f.Chave)
            .ToListAsync(cancellationToken);
    }

    public async Task<TbFeatureFlag?> GetByChaveAsync(string chave, CancellationToken cancellationToken = default)
    {
        return await _context.FeatureFlags
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Chave == chave, cancellationToken);
    }

    public async Task<bool> IsEnabledAsync(string chave, CancellationToken cancellationToken = default)
    {
        var flag = await _context.FeatureFlags
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Chave == chave, cancellationToken);
        return flag?.Habilitado ?? false;
    }

    public async Task UpdateAsync(TbFeatureFlag featureFlag, CancellationToken cancellationToken = default)
    {
        featureFlag.DataAtualizacao = DateTime.UtcNow;
        _context.FeatureFlags.Update(featureFlag);
    }
}

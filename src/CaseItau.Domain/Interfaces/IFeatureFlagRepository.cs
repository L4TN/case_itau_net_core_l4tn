using CaseItau.Domain.Entities;

namespace CaseItau.Domain.Interfaces;

public interface IFeatureFlagRepository
{
    Task<IEnumerable<TbFeatureFlag>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TbFeatureFlag?> GetByChaveAsync(string chave, CancellationToken cancellationToken = default);
    Task<bool> IsEnabledAsync(string chave, CancellationToken cancellationToken = default);
    Task UpdateAsync(TbFeatureFlag featureFlag, CancellationToken cancellationToken = default);
}

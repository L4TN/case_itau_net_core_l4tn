using CaseItau.Application.DTOs.FeatureFlag;

namespace CaseItau.Application.Services.Interfaces;

public interface IFeatureFlagService
{
    Task<IEnumerable<FeatureFlagResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FeatureFlagResponseDto?> GetByChaveAsync(string chave, CancellationToken cancellationToken = default);
    Task<bool> IsEnabledAsync(string chave, CancellationToken cancellationToken = default);
    Task<FeatureFlagResponseDto> ToggleAsync(string chave, bool habilitado, CancellationToken cancellationToken = default);
}

using AutoMapper;
using CaseItau.Application.DTOs.FeatureFlag;
using CaseItau.Application.Services.Interfaces;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CaseItau.Application.Services;

public class FeatureFlagService : IFeatureFlagService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<FeatureFlagService> _logger;

    public FeatureFlagService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FeatureFlagService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<FeatureFlagResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var flags = await _unitOfWork.FeatureFlags.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<FeatureFlagResponseDto>>(flags);
    }

    public async Task<FeatureFlagResponseDto?> GetByChaveAsync(string chave, CancellationToken cancellationToken = default)
    {
        var flag = await _unitOfWork.FeatureFlags.GetByChaveAsync(chave, cancellationToken);
        return flag is null ? null : _mapper.Map<FeatureFlagResponseDto>(flag);
    }

    public async Task<bool> IsEnabledAsync(string chave, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.FeatureFlags.IsEnabledAsync(chave, cancellationToken);
    }

    public async Task<FeatureFlagResponseDto> ToggleAsync(string chave, bool habilitado, CancellationToken cancellationToken = default)
    {
        var flag = await _unitOfWork.FeatureFlags.GetByChaveAsync(chave, cancellationToken)
            ?? throw new NotFoundException("FeatureFlag", chave);

        flag.Habilitado = habilitado;
        await _unitOfWork.FeatureFlags.UpdateAsync(flag, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Feature flag '{Chave}' alterada para {Estado}.", chave, habilitado ? "HABILITADO" : "DESABILITADO");

        return _mapper.Map<FeatureFlagResponseDto>(flag);
    }
}

using AutoMapper;
using CaseItau.Application.DTOs.TipoFundo;
using CaseItau.Application.Services.Interfaces;
using CaseItau.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CaseItau.Application.Services;

public class TipoFundoService : ITipoFundoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<TipoFundoService> _logger;

    public TipoFundoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TipoFundoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TipoFundoResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Listando todos os tipos de fundo.");
        var tipos = await _unitOfWork.TiposFundo.GetAllTpFundsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TipoFundoResponseDto>>(tipos);
    }
}

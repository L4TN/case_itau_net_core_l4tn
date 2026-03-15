using CaseItau.Application.DTOs.TipoFundo;

namespace CaseItau.Application.Services.Interfaces;

public interface ITipoFundoService
{
    Task<IEnumerable<TipoFundoResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

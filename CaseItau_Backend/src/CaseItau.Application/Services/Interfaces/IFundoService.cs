using CaseItau.Application.DTOs.Common;
using CaseItau.Application.DTOs.Fundo;

namespace CaseItau.Application.Services.Interfaces;

public interface IFundoService
{
    Task<IEnumerable<FundoResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<FundoResponseDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<FundoResponseDto> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<FundoResponseDto> CreateAsync(CreateFundoRequestDto dto, CancellationToken cancellationToken = default);
    Task<FundoResponseDto> UpdateAsync(string codigo, UpdateFundoRequestDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(string codigo, CancellationToken cancellationToken = default);
}

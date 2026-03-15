using CaseItau.Application.DTOs.Movimentacao;

namespace CaseItau.Application.Services.Interfaces;

public interface IMovimentacaoService
{
    Task<PosicaoFundoResponseDto> MovimentarAsync(string codigoFundo, CreateMovimentacaoRequestDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<PosicaoFundoResponseDto>> GetEvolucaoPatrimonialAsync(string codigoFundo, CancellationToken cancellationToken = default);
    Task<IEnumerable<MovimentacaoResponseDto>> GetMovimentacoesByFundoAsync(string codigoFundo, CancellationToken cancellationToken = default);
}

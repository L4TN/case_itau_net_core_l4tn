using CaseItau.Application.DTOs.Movimentacao;
using CaseItau.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseItau.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class MovimentacaoController : ControllerBase
{
    private readonly IMovimentacaoService _movimentacaoService;

    public MovimentacaoController(IMovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
    }

    [HttpPost("{codigoFundo}")]
    public async Task<ActionResult<PosicaoFundoResponseDto>> Post(string codigoFundo, [FromBody] CreateMovimentacaoRequestDto dto, CancellationToken cancellationToken)
    {
        var posicao = await _movimentacaoService.MovimentarAsync(codigoFundo, dto, cancellationToken);
        return Created(string.Empty, posicao);
    }

    [HttpGet("{codigoFundo}/evolucao-patrimonial")]
    public async Task<ActionResult<IEnumerable<PosicaoFundoResponseDto>>> GetEvolucaoPatrimonial(string codigoFundo, CancellationToken cancellationToken)
    {
        var evolucao = await _movimentacaoService.GetEvolucaoPatrimonialAsync(codigoFundo, cancellationToken);
        return Ok(evolucao);
    }

    [HttpGet("{codigoFundo}")]
    public async Task<ActionResult<IEnumerable<MovimentacaoResponseDto>>> GetMovimentacoes(string codigoFundo, CancellationToken cancellationToken)
    {
        var movimentacoes = await _movimentacaoService.GetMovimentacoesByFundoAsync(codigoFundo, cancellationToken);
        return Ok(movimentacoes);
    }
}

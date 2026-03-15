using CaseItau.Application.DTOs.Fundo;
using CaseItau.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseItau.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class FundoController : ControllerBase
{
    private readonly IFundoService _fundoService;

    public FundoController(IFundoService fundoService)
    {
        _fundoService = fundoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFunds(CancellationToken cancellationToken)
    {
        var fundos = await _fundoService.GetAllAsync(cancellationToken);
        return Ok(fundos);
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<FundoResponseDto>> GetFundByCodigo(string codigo, CancellationToken cancellationToken)
    {
        var fundo = await _fundoService.GetByCodigoAsync(codigo, cancellationToken);
        return Ok(fundo);
    }

    [HttpPost]
    public async Task<ActionResult<FundoResponseDto>> Post([FromBody] CreateFundoRequestDto dto, CancellationToken cancellationToken)
    {
        var fundo = await _fundoService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetFundByCodigo), new { codigo = fundo.CdFundo }, fundo);
    }

    [HttpPut("{codigo}")]
    public async Task<ActionResult<FundoResponseDto>> Put(string codigo, [FromBody] UpdateFundoRequestDto dto, CancellationToken cancellationToken)
    {
        var fundo = await _fundoService.UpdateAsync(codigo, dto, cancellationToken);
        return Ok(fundo);
    }

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> Delete(string codigo, CancellationToken cancellationToken)
    {
        await _fundoService.DeleteAsync(codigo, cancellationToken);
        return NoContent();
    }
}

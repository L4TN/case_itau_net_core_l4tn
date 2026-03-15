using CaseItau.Application.DTOs.TipoFundo;
using CaseItau.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseItau.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class TipoFundoController : ControllerBase
{
    private readonly ITipoFundoService _tipoFundoService;

    public TipoFundoController(ITipoFundoService tipoFundoService)
    {
        _tipoFundoService = tipoFundoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TipoFundoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TipoFundoResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var tipos = await _tipoFundoService.GetAllAsync(cancellationToken);
        return Ok(tipos);
    }
}

using CaseItau.Application.DTOs.FeatureFlag;
using CaseItau.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaseItau.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class FeatureFlagController : ControllerBase
{
    private readonly IFeatureFlagService _featureFlagService;

    public FeatureFlagController(IFeatureFlagService featureFlagService)
    {
        _featureFlagService = featureFlagService;
    }

    #region GET

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeatureFlagResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var flags = await _featureFlagService.GetAllAsync(cancellationToken);
        return Ok(flags);
    }

    [HttpGet("{chave}")]
    public async Task<ActionResult<FeatureFlagResponseDto>> GetByChave(string chave, CancellationToken cancellationToken)
    {
        var flag = await _featureFlagService.GetByChaveAsync(chave, cancellationToken);
        if (flag is null)
            return NotFound();
        return Ok(flag);
    }

    [HttpGet("{chave}/enabled")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> IsEnabled(string chave, CancellationToken cancellationToken)
    {
        var enabled = await _featureFlagService.IsEnabledAsync(chave, cancellationToken);
        return Ok(enabled);
    }

    #endregion

    #region PUT

    [HttpPut("{chave}/toggle")]
    public async Task<ActionResult<FeatureFlagResponseDto>> Toggle(string chave, [FromQuery] bool habilitado, CancellationToken cancellationToken)
    {
        var flag = await _featureFlagService.ToggleAsync(chave, habilitado, cancellationToken);
        return Ok(flag);
    }

    #endregion
}

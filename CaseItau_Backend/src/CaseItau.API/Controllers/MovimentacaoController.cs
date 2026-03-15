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
    private readonly IRedisCacheService _cacheService;
    private readonly IFeatureFlagService _featureFlagService;
    private readonly TimeSpan _redisExpiration;

    private const string RedisKeyMovimentacoesPrefix = "movimentacoes:";
    private const string RedisKeyPosicoesPrefix = "posicoes:";
    private const string FeatureFlagCacheRedis = "cache_redis";

    public MovimentacaoController(
        IMovimentacaoService movimentacaoService,
        IRedisCacheService cacheService,
        IFeatureFlagService featureFlagService,
        IConfiguration configuration)
    {
        _movimentacaoService = movimentacaoService;
        _cacheService = cacheService;
        _featureFlagService = featureFlagService;
        _redisExpiration = TimeSpan.FromSeconds(configuration.GetValue<int>("Redis:ExpirationInSeconds", 60));
    }

    #region POST

    [HttpPost("{codigoFundo}")]
    public async Task<ActionResult<PosicaoFundoResponseDto>> Post(string codigoFundo, [FromBody] CreateMovimentacaoRequestDto dto, CancellationToken cancellationToken)
    {
        var posicao = await _movimentacaoService.MovimentarAsync(codigoFundo, dto, cancellationToken);

        if (await _featureFlagService.IsEnabledAsync(FeatureFlagCacheRedis, cancellationToken))
        {
            await InvalidateRedisCacheAsync(codigoFundo);
        }

        return Created(string.Empty, posicao);
    }

    #endregion

    #region GET

    [HttpGet("{codigoFundo}/evolucao-patrimonial")]
    public async Task<ActionResult<IEnumerable<PosicaoFundoResponseDto>>> GetEvolucaoPatrimonial(
        string codigoFundo,
        [FromQuery] bool cacheRedis = false,
        CancellationToken cancellationToken = default)
    {
        var cacheHabilitado = cacheRedis && await _featureFlagService.IsEnabledAsync(FeatureFlagCacheRedis, cancellationToken);
        var redisKey = $"{RedisKeyPosicoesPrefix}{codigoFundo}";

        if (cacheHabilitado)
        {
            var cached = await _cacheService.GetAsync<IEnumerable<PosicaoFundoResponseDto>>(redisKey, cancellationToken);
            if (cached is not null)
                return Ok(cached);
        }

        var evolucao = await _movimentacaoService.GetEvolucaoPatrimonialAsync(codigoFundo, cancellationToken);

        if (cacheHabilitado)
        {
            await _cacheService.SetAsync(redisKey, evolucao, _redisExpiration, cancellationToken);
        }

        return Ok(evolucao);
    }

    [HttpGet("{codigoFundo}")]
    public async Task<ActionResult<IEnumerable<MovimentacaoResponseDto>>> GetMovimentacoes(
        string codigoFundo,
        [FromQuery] bool cacheRedis = false,
        CancellationToken cancellationToken = default)
    {
        var cacheHabilitado = cacheRedis && await _featureFlagService.IsEnabledAsync(FeatureFlagCacheRedis, cancellationToken);
        var redisKey = $"{RedisKeyMovimentacoesPrefix}{codigoFundo}";

        if (cacheHabilitado)
        {
            var cached = await _cacheService.GetAsync<IEnumerable<MovimentacaoResponseDto>>(redisKey, cancellationToken);
            if (cached is not null)
                return Ok(cached);
        }

        var movimentacoes = await _movimentacaoService.GetMovimentacoesByFundoAsync(codigoFundo, cancellationToken);

        if (cacheHabilitado)
        {
            await _cacheService.SetAsync(redisKey, movimentacoes, _redisExpiration, cancellationToken);
        }

        return Ok(movimentacoes);
    }

    #endregion

    #region Private

    private async Task InvalidateRedisCacheAsync(string codigoFundo)
    {
        await _cacheService.RemoveAsync($"{RedisKeyMovimentacoesPrefix}{codigoFundo}");
        await _cacheService.RemoveAsync($"{RedisKeyPosicoesPrefix}{codigoFundo}");
    }

    #endregion
}

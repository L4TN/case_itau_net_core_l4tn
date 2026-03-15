using System.Text.Json;
using CaseItau.Application.Services.Interfaces;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using StackExchange.Redis;

namespace CaseItau.API.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _redisDb;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly ResiliencePipeline _resiliencePipeline;

    public RedisCacheService(
        IConnectionMultiplexer redis,
        ILogger<RedisCacheService> logger)
    {
        _redisDb = redis.GetDatabase();
        _logger = logger;

        _resiliencePipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<RedisConnectionException>().Handle<RedisTimeoutException>(),
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(200),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    logger.LogWarning("Redis retry {AttemptNumber} após {Delay}ms: {Error}",
                        args.AttemptNumber, args.RetryDelay.TotalMilliseconds, args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                }
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<RedisConnectionException>().Handle<RedisTimeoutException>(),
                FailureRatio = 0.5,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(30),
                OnOpened = args =>
                {
                    logger.LogError("Circuit ABERTO para Redis por {Duration}s. Fallback para banco direto.",
                        args.BreakDuration.TotalSeconds);
                    return ValueTask.CompletedTask;
                },
                OnClosed = args =>
                {
                    logger.LogInformation("Circuit FECHADO para Redis — conexão restabelecida.");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = args =>
                {
                    logger.LogInformation("Circuit HALF-OPEN para Redis — testando reconexão.");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var result = await _resiliencePipeline.ExecuteAsync(
                async ct => await _redisDb.StringGetAsync(key),
                cancellationToken);

            if (!result.HasValue)
                return null;

            return JsonSerializer.Deserialize<T>(result!);
        }
        catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException or BrokenCircuitException)
        {
            _logger.LogWarning("Redis indisponível para GET '{Key}'. Fallback para banco. Erro: {Error}", key, ex.Message);
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var json = JsonSerializer.Serialize(value);
            await _resiliencePipeline.ExecuteAsync(
                async ct => await _redisDb.StringSetAsync(key, json, expiration),
                cancellationToken);
        }
        catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException or BrokenCircuitException)
        {
            _logger.LogWarning("Redis indisponível para SET '{Key}'. Cache ignorado. Erro: {Error}", key, ex.Message);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _resiliencePipeline.ExecuteAsync(
                async ct => await _redisDb.KeyDeleteAsync(key),
                cancellationToken);
        }
        catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException or BrokenCircuitException)
        {
            _logger.LogWarning("Redis indisponível para DELETE '{Key}'. Erro: {Error}", key, ex.Message);
        }
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _resiliencePipeline.ExecuteAsync(
                async ct => await _redisDb.KeyExistsAsync(key),
                cancellationToken);
        }
        catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException or BrokenCircuitException)
        {
            _logger.LogWarning("Redis indisponível para EXISTS '{Key}'. Erro: {Error}", key, ex.Message);
            return false;
        }
    }
}

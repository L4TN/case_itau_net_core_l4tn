using System.Text.Json;
using CaseItau.Application.Services.Interfaces;
using StackExchange.Redis;

namespace CaseItau.API.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _redisDb;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(
        IConnectionMultiplexer redis,
        ILogger<RedisCacheService> logger)
    {
        _redisDb = redis.GetDatabase();
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var result = await _redisDb.StringGetAsync(key);
            if (!result.HasValue)
                return null;
            return JsonSerializer.Deserialize<T>(result!);
        }
        catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException)
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
            await _redisDb.StringSetAsync(key, json, expiration);
        }
        catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException)
        {
            _logger.LogWarning("Redis indisponível para SET '{Key}'. Cache ignorado. Erro: {Error}", key, ex.Message);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _redisDb.KeyDeleteAsync(key);
        }
        catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException)
        {
            _logger.LogWarning("Redis indisponível para DELETE '{Key}'. Erro: {Error}", key, ex.Message);
        }
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _redisDb.KeyExistsAsync(key);
        }
        catch (Exception ex) when (ex is RedisConnectionException or RedisTimeoutException)
        {
            _logger.LogWarning("Redis indisponível para EXISTS '{Key}'. Erro: {Error}", key, ex.Message);
            return false;
        }
    }
}

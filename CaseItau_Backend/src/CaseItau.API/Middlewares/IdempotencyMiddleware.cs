using CaseItau.Application.Services.Interfaces;

namespace CaseItau.API.Middlewares;

public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IdempotencyMiddleware> _logger;

    public IdempotencyMiddleware(RequestDelegate next, ILogger<IdempotencyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IRedisCacheService cacheService)
    {
        if (context.Request.Method is not ("POST" or "PUT" or "PATCH"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey)
            || string.IsNullOrWhiteSpace(idempotencyKey))
        {
            await _next(context);
            return;
        }

        var cacheKey = $"idempotency:{idempotencyKey}";

        var cachedResponse = await cacheService.GetAsync<IdempotentResponse>(cacheKey);
        if (cachedResponse is not null)
        {
            _logger.LogInformation("Idempotency-Key '{Key}' já processada. Retornando resposta cacheada.", idempotencyKey.ToString());
            context.Response.StatusCode = cachedResponse.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(cachedResponse.Body);
            return;
        }

        var originalBody = context.Response.Body;
        using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        await _next(context);

        memStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(memStream).ReadToEndAsync();

        if (context.Response.StatusCode is >= 200 and < 300)
        {
            var idempotentResponse = new IdempotentResponse
            {
                StatusCode = context.Response.StatusCode,
                Body = responseBody
            };

            await cacheService.SetAsync(cacheKey, idempotentResponse, TimeSpan.FromHours(24));
            _logger.LogInformation("Idempotency-Key '{Key}' armazenada no cache Redis.", idempotencyKey.ToString());
        }

        memStream.Seek(0, SeekOrigin.Begin);
        await memStream.CopyToAsync(originalBody);
        context.Response.Body = originalBody;
    }
}

internal class IdempotentResponse
{
    public int StatusCode { get; init; }
    public string Body { get; init; } = "";
}

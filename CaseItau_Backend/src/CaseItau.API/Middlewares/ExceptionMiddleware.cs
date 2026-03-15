using CaseItau.Domain.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog.Context;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace CaseItau.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var usuario = context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "anonymous";
        var requestPath = context.Request.Path;
        var method = context.Request.Method;

        using (LogContext.PushProperty("Usuario", usuario))
        using (LogContext.PushProperty("RequestPath", requestPath))
        using (LogContext.PushProperty("HttpMethod", method))
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Exceção não tratada: {ExceptionType} — {Message} | Usuário: {Usuario} | {HttpMethod} {RequestPath}",
                    ex.GetType().Name, ex.Message, usuario, method, requestPath);
                await HandleExceptionAsync(context, ex);
            }
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            NotFoundException notFound => (HttpStatusCode.NotFound, notFound.Message),
            DomainException domain => (HttpStatusCode.BadRequest, domain.Message),
            ValidationException validation => (HttpStatusCode.BadRequest,
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))),
            DbUpdateConcurrencyException => (HttpStatusCode.Conflict,
                "Conflito de concorrência: outro request alterou este recurso simultaneamente. Tente novamente."),
            _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro interno no servidor.")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = (int)statusCode,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}

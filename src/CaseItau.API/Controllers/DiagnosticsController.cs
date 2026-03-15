using CaseItau.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class DiagnosticsController : ControllerBase
{
    private readonly ILogger<DiagnosticsController> _logger;

    public DiagnosticsController(ILogger<DiagnosticsController> logger)
    {
        _logger = logger;
    }

    [HttpPost("logs/all-levels")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GenerateAllLogLevels()
    {
        var usuario = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "unknown";

        _logger.LogDebug("[DIAGNOSTICS] Log DEBUG — Usuário: {Usuario} — Detalhe técnico de debug", usuario);
        _logger.LogInformation("[DIAGNOSTICS] Log INFORMATION — Usuário: {Usuario} — Operação de negócio executada", usuario);
        _logger.LogWarning("[DIAGNOSTICS] Log WARNING — Usuário: {Usuario} — Cache Redis miss, fallback para SQL Server", usuario);
        _logger.LogError("[DIAGNOSTICS] Log ERROR — Usuário: {Usuario} — Falha ao processar movimentação de fundo", usuario);
        _logger.LogCritical("[DIAGNOSTICS] Log CRITICAL — Usuário: {Usuario} — Conexão com banco de dados perdida", usuario);

        return Ok(new
        {
            Message = "5 logs gerados com sucesso (Debug, Information, Warning, Error, Critical)",
            Usuario = usuario,
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("errors/not-found")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult SimulateNotFound()
    {
        throw new NotFoundException("TbFundo", "FUNDO_INEXISTENTE_123");
    }

    [HttpGet("errors/domain")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SimulateDomainException()
    {
        throw new DomainException("Valor de movimentação não pode ser negativo. Valor recebido: -500000.00");
    }

    [HttpGet("errors/validation")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult SimulateValidationException()
    {
        var failures = new List<FluentValidation.Results.ValidationFailure>
        {
            new("Cnpj", "CNPJ inválido: '00000000000000'. Todos os dígitos são iguais."),
            new("Nome", "Nome do fundo é obrigatório.")
        };
        throw new FluentValidation.ValidationException(failures);
    }

    [HttpGet("errors/concurrency")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult SimulateConcurrencyException()
    {
        throw new DbUpdateConcurrencyException(
            "A database operation failed. Another user modified the record concurrently.");
    }

    [HttpGet("errors/internal")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult SimulateInternalError()
    {
        throw new InvalidOperationException(
            "Conexão com serviço externo de compliance timeout após 30s — operação cancelada.");
    }

    [HttpPost("logs/bulk-errors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GenerateBulkErrors([FromQuery] int count = 10)
    {
        var clamped = Math.Clamp(count, 1, 100);
        var usuario = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "unknown";

        for (var i = 1; i <= clamped; i++)
        {
            _logger.LogError(
                "[DIAGNOSTICS] Erro simulado {ErrorNumber}/{Total} — Usuário: {Usuario} — " +
                "Falha ao processar movimentação do fundo ITAU{FundoId:D3}. Timeout na consulta SQL após 5000ms.",
                i, clamped, usuario, i);
        }

        _logger.LogWarning(
            "[DIAGNOSTICS] Circuit Breaker ABERTO para Redis por 30s — {ErrorCount} falhas consecutivas. " +
            "Fallback ativo: consultas diretas ao SQL Server. Usuário: {Usuario}",
            clamped, usuario);

        return Ok(new
        {
            Message = $"{clamped} logs de erro + 1 warning de circuit breaker gerados",
            Usuario = usuario,
            Timestamp = DateTime.UtcNow
        });
    }
}

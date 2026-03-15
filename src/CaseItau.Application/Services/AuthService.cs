using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CaseItau.Application.DTOs.Auth;
using CaseItau.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CaseItau.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly ITokenCryptoService _tokenCrypto;

    public AuthService(IConfiguration configuration, ILogger<AuthService> logger, ITokenCryptoService tokenCrypto)
    {
        _configuration = configuration;
        _logger = logger;
        _tokenCrypto = tokenCrypto;
    }

    public LoginResponseDto? Authenticate(LoginRequestDto request)
    {
        _logger.LogInformation("Tentativa de login para o usuário {Usuario}.", request.Usuario);

        if (request.Usuario != "admin" || request.Senha != "admin123")
        {
            _logger.LogWarning("Credenciais inválidas para o usuário {Usuario}.", request.Usuario);
            return null;
        }

        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationInMinutes"]!);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Usuario),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        var encryptedToken = _tokenCrypto.Encrypt(tokenString);

        _logger.LogInformation("Token JWT gerado com sucesso para o usuário {Usuario}. Expira em {Expiration}.", request.Usuario, expiration);

        return new LoginResponseDto
        {
            Token = encryptedToken,
            Expiration = expiration
        };
    }
}

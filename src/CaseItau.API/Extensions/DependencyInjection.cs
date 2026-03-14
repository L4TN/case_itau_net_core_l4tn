using System.Text;
using CaseItau.API.Services;
using CaseItau.Application.Mappings;
using CaseItau.Application.Services;
using CaseItau.Application.Services.Interfaces;
using CaseItau.Application.Validators;
using CaseItau.Domain.Interfaces;
using CaseItau.Infra.Data;
using CaseItau.Infra.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace CaseItau.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DboContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(DboContext).Assembly.FullName)));

        services.AddScoped<IFundoRepository, FundoRepository>();
        services.AddScoped<ITipoFundoRepository, TipoFundoRepository>();
        services.AddScoped<IPosicaoFundoRepository, PosicaoFundoRepository>();
        services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
        services.AddScoped<IFeatureFlagRepository, FeatureFlagRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IFundoService, FundoService>();
        services.AddScoped<ITipoFundoService, TipoFundoService>();
        services.AddScoped<IMovimentacaoService, MovimentacaoService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IFeatureFlagService, FeatureFlagService>();
        services.AddSingleton<ITokenCryptoService, TokenCryptoService>();

        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateFundoValidator>();

        var redisConnectionString = configuration["Redis:ConnectionString"] ?? "localhost:6379";
        var redisEnabled = configuration.GetValue<bool>("Redis:Enabled");
        if (redisEnabled)
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnectionString));
        }
        else
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false"));
        }

        services.AddSingleton<IRedisCacheService, RedisCacheService>();

        var jwtKey = configuration["Jwt:Key"]!;
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;

        var tokenCrypto = new TokenCryptoService(configuration);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
                    if (authHeader is not null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        var encryptedToken = authHeader["Bearer ".Length..].Trim();
                        try
                        {
                            context.Token = tokenCrypto.Decrypt(encryptedToken);
                        }
                        catch
                        {
                        }
                    }
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}

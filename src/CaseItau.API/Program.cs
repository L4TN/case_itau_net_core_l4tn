using System.Threading.RateLimiting;
using AWS.Logger;
using AWS.Logger.SeriLog;
using CaseItau.API.Extensions;
using CaseItau.API.Middlewares;
using CaseItau.Infra.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var awsCloudWatchEnabled = builder.Configuration.GetValue<bool>("AWS:CloudWatch:Enabled");

var loggerConfig = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithThreadId()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}");

if (awsCloudWatchEnabled)
{
    var awsLogConfig = new AWSLoggerConfig
    {
        LogGroup = builder.Configuration["AWS:CloudWatch:LogGroup"] ?? "/caseitau/api",
        Region = builder.Configuration["AWS:CloudWatch:Region"] ?? "us-east-1"
    };
    loggerConfig.WriteTo.AWSSeriLog(awsLogConfig);
}

Log.Logger = loggerConfig.CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Case Itau - API de Fundos",
        Version = "v1",
        Description = "API para gerenciamento de fundos de investimento - Case tecnico Itau Asset Management"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            StatusCode = 429,
            Message = "Limite de requisições excedido. Tente novamente em 1 minuto.",
            Timestamp = DateTime.UtcNow
        }, cancellationToken);
    };
});

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DboContext>();
    db.Database.Migrate();
    Log.Information("Migrations aplicadas com sucesso.");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Case Itau API v1"));
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            Status = report.Status.ToString(),
            Duration = report.TotalDuration.TotalMilliseconds + "ms",
            Checks = report.Entries.Select(e => new
            {
                Name = e.Key,
                Status = e.Value.Status.ToString(),
                Duration = e.Value.Duration.TotalMilliseconds + "ms",
                Description = e.Value.Description ?? ""
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

app.UseSerilogRequestLogging();
Log.Information("API Case Itau iniciada com sucesso.");
app.Run();

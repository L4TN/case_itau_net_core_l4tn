using CaseItau.Application.DTOs.Auth;

namespace CaseItau.Application.Services.Interfaces;

public interface IAuthService
{
    LoginResponseDto? Authenticate(LoginRequestDto request);
}

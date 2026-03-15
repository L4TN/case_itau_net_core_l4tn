using CaseItau.Application.DTOs.Auth;
using FluentValidation;

namespace CaseItau.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Usuario)
            .NotEmpty().WithMessage("Usuário é obrigatório.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória.");
    }
}

using CaseItau.Application.DTOs.Fundo;
using FluentValidation;

namespace CaseItau.Application.Validators;

public class CreateFundoValidator : AbstractValidator<CreateFundoRequestDto>
{
    public CreateFundoValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("Código é obrigatório.")
            .MaximumLength(20).WithMessage("Código deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("CNPJ é obrigatório.")
            .Must(CnpjValidator.IsValid).WithMessage("CNPJ inválido.");

        RuleFor(x => x.TipoFundoId)
            .GreaterThan(0).WithMessage("Id do tipo de fundo é obrigatório.");
    }
}

using CaseItau.Application.DTOs.Fundo;
using FluentValidation;

namespace CaseItau.Application.Validators;

public class UpdateFundoValidator : AbstractValidator<UpdateFundoRequestDto>
{
    public UpdateFundoValidator()
    {
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

using CaseItau.Application.DTOs.Movimentacao;
using FluentValidation;

namespace CaseItau.Application.Validators;

public class CreateMovimentacaoValidator : AbstractValidator<CreateMovimentacaoRequestDto>
{
    public CreateMovimentacaoValidator()
    {
        RuleFor(x => x.Valor)
            .NotEqual(0).WithMessage("Valor da movimentação não pode ser zero.");
    }
}

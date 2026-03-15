using CaseItau.Application.DTOs.Movimentacao;
using CaseItau.Application.Validators;
using FluentAssertions;
using Xunit;

namespace CaseItau.Tests.Validators;

public class CreateMovimentacaoValidatorTests
{
    private readonly CreateMovimentacaoValidator _validator = new();

    [Theory]
    [InlineData(100)]
    [InlineData(-50)]
    [InlineData(0.01)]
    public void QuandoValorDiferenteDeZero_NaoDeveRetornarErro(decimal valor)
    {
        var dto = new CreateMovimentacaoRequestDto { Valor = valor };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void QuandoValorZero_DeveRetornarErro()
    {
        var dto = new CreateMovimentacaoRequestDto { Valor = 0 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }
}

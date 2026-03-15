using CaseItau.Application.DTOs.Fundo;
using CaseItau.Application.Validators;
using FluentAssertions;
using Xunit;

namespace CaseItau.Tests.Validators;

public class UpdateFundoValidatorTests
{
    private readonly UpdateFundoValidator _validator = new();

    [Fact]
    public void QuandoTodosOsCamposValidos_NaoDeveRetornarErros()
    {
        var dto = new UpdateFundoRequestDto { Nome = "Fundo Teste", Cnpj = "11222333000181", TipoFundoId = 1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void QuandoNomeVazio_DeveRetornarErro()
    {
        var dto = new UpdateFundoRequestDto { Nome = "", Cnpj = "11222333000181", TipoFundoId = 1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("1234567890123")]
    [InlineData("112223330001815")]
    [InlineData("00000000000000")]
    public void QuandoCnpjInvalido_DeveRetornarErro(string cnpj)
    {
        var dto = new UpdateFundoRequestDto { Nome = "Fundo Teste", Cnpj = cnpj, TipoFundoId = 1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Cnpj");
    }

    [Fact]
    public void QuandoTipoFundoIdZeroOuNegativo_DeveRetornarErro()
    {
        var dto = new UpdateFundoRequestDto { Nome = "Fundo Teste", Cnpj = "11222333000181", TipoFundoId = 0 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "TipoFundoId");
    }
}

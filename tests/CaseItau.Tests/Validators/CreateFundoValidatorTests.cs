using CaseItau.Application.DTOs.Fundo;
using CaseItau.Application.Validators;
using FluentAssertions;
using Xunit;

namespace CaseItau.Tests.Validators;

public class CreateFundoValidatorTests
{
    private readonly CreateFundoValidator _validator = new();

    [Fact]
    public void QuandoTodosOsCamposValidos_NaoDeveRetornarErros()
    {
        var dto = new CreateFundoRequestDto { Codigo = "ITAUTEST01", Nome = "Fundo Teste", Cnpj = "11222333000181", TipoFundoId = 1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Código é obrigatório")]
    [InlineData(null, "Código é obrigatório")]
    public void QuandoCodigoInvalido_DeveRetornarErro(string? codigo, string mensagemEsperada)
    {
        var dto = new CreateFundoRequestDto { Codigo = codigo!, Nome = "X", Cnpj = "11222333000181", TipoFundoId = 1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains(mensagemEsperada));
    }

    [Fact]
    public void QuandoCodigoExcede20Caracteres_DeveRetornarErro()
    {
        var dto = new CreateFundoRequestDto { Codigo = new string('X', 21), Nome = "Fundo", Cnpj = "11222333000181", TipoFundoId = 1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void QuandoNomeVazio_DeveRetornarErro()
    {
        var dto = new CreateFundoRequestDto { Codigo = "X", Nome = "", Cnpj = "11222333000181", TipoFundoId = 1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("1234567890123")]
    [InlineData("112223330001815")]
    [InlineData("1234567890ABCD")]
    public void QuandoCnpjInvalido_DeveRetornarErro(string cnpj)
    {
        var dto = new CreateFundoRequestDto { Codigo = "X", Nome = "Y", Cnpj = cnpj, TipoFundoId = 1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void QuandoTipoFundoIdZeroOuNegativo_DeveRetornarErro()
    {
        var dto = new CreateFundoRequestDto { Codigo = "X", Nome = "Y", Cnpj = "11222333000181", TipoFundoId = 0 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }
}

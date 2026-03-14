using AutoMapper;
using CaseItau.Application.Mappings;
using CaseItau.Application.Services;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CaseItau.Tests.Services;

public class TipoFundoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITipoFundoRepository> _tipoFundoRepoMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<TipoFundoService>> _loggerMock;
    private readonly TipoFundoService _service;

    public TipoFundoServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _tipoFundoRepoMock = new Mock<ITipoFundoRepository>();
        _loggerMock = new Mock<ILogger<TipoFundoService>>();

        _unitOfWorkMock.Setup(u => u.TiposFundo).Returns(_tipoFundoRepoMock.Object);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _service = new TipoFundoService(_unitOfWorkMock.Object, _mapper, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaDeTipos()
    {
        var tipos = new List<TbTipoFundo>
        {
            new() { Id = 1, Nome = "RENDA FIXA" },
            new() { Id = 2, Nome = "ACOES" }
        };
        _tipoFundoRepoMock.Setup(r => r.GetAllTpFundsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tipos);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().Contain(t => t.NmTipoFundo == "RENDA FIXA");
        result.Should().Contain(t => t.NmTipoFundo == "ACOES");
    }

    [Fact]
    public async Task GetAllAsync_QuandoNaoHaTipos_DeveRetornarListaVazia()
    {
        _tipoFundoRepoMock.Setup(r => r.GetAllTpFundsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<TbTipoFundo>());

        var result = await _service.GetAllAsync();

        result.Should().BeEmpty();
    }
}

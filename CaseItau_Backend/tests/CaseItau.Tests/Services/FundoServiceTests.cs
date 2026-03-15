using AutoMapper;
using CaseItau.Application.DTOs.Fundo;
using CaseItau.Application.Mappings;
using CaseItau.Application.Services;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CaseItau.Tests.Services;

public class FundoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IFundoRepository> _fundoRepoMock;
    private readonly Mock<ITipoFundoRepository> _tipoFundoRepoMock;
    private readonly Mock<IPosicaoFundoRepository> _posicaoRepoMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<FundoService>> _loggerMock;
    private readonly FundoService _service;

    public FundoServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _fundoRepoMock = new Mock<IFundoRepository>();
        _tipoFundoRepoMock = new Mock<ITipoFundoRepository>();
        _posicaoRepoMock = new Mock<IPosicaoFundoRepository>();
        _loggerMock = new Mock<ILogger<FundoService>>();

        _unitOfWorkMock.Setup(u => u.Fundos).Returns(_fundoRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.TiposFundo).Returns(_tipoFundoRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.PosicoesFundo).Returns(_posicaoRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _service = new FundoService(
            _unitOfWorkMock.Object,
            _mapper,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaDeFundos()
    {
        var fundos = new List<TbFundo>
        {
            new() { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1, TipoFundo = new TbTipoFundo { Id = 1, Nome = "RENDA FIXA" } },
            new() { Id = 2, Codigo = "F2", Nome = "Fundo 2", Cnpj = "22222222222222", TipoFundoId = 2, TipoFundo = new TbTipoFundo { Id = 2, Nome = "ACOES" } }
        };
        _fundoRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(fundos);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().Contain(f => f.NmTipoFundo == "RENDA FIXA");
        result.Should().Contain(f => f.NmTipoFundo == "ACOES");
    }

    [Fact]
    public async Task GetAllAsync_QuandoNaoHaFundos_DeveRetornarListaVazia()
    {
        _fundoRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<TbFundo>());

        var result = await _service.GetAllAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByCodigoAsync_QuandoFundoExiste_DeveRetornarFundo()
    {
        var fundo = new TbFundo
        {
            Id = 1, Codigo = "ITAURF123", Nome = "Test Fund", Cnpj = "11111111111111",
            TipoFundoId = 1,
            TipoFundo = new TbTipoFundo { Id = 1, Nome = "RENDA FIXA" }
        };
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("ITAURF123", It.IsAny<CancellationToken>())).ReturnsAsync(fundo);

        var result = await _service.GetByCodigoAsync("ITAURF123");

        result.Should().NotBeNull();
        result.CdFundo.Should().Be("ITAURF123");
    }

    [Fact]
    public async Task GetByCodigoAsync_QuandoFundoNaoExiste_DeveLancarNotFoundException()
    {
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("INEXISTENTE", It.IsAny<CancellationToken>())).ReturnsAsync((TbFundo?)null);

        var act = () => _service.GetByCodigoAsync("INEXISTENTE");

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("*INEXISTENTE*");
    }

    [Fact]
    public async Task CreateAsync_QuandoDadosValidos_DeveCriarFundo()
    {
        var dto = new CreateFundoRequestDto { Codigo = "NOVOFUNDO", Nome = "Fundo Novo", Cnpj = "99999999999999", TipoFundoId = 1 };

        _fundoRepoMock.Setup(r => r.ExistsByCodigoAsync(dto.Codigo, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _fundoRepoMock.Setup(r => r.ExistsByCnpjAsync(dto.Cnpj, null, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _tipoFundoRepoMock.Setup(r => r.ExistsFundByIdAsync(dto.TipoFundoId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _fundoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbFundo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("NOVOFUNDO", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TbFundo { Id = 1, Codigo = "NOVOFUNDO", Nome = "Fundo Novo", Cnpj = "99999999999999", TipoFundoId = 1, TipoFundo = new TbTipoFundo { Id = 1, Nome = "RENDA FIXA" } });

        var result = await _service.CreateAsync(dto);

        result.CdFundo.Should().Be("NOVOFUNDO");
        result.NmTipoFundo.Should().Be("RENDA FIXA");
        _fundoRepoMock.Verify(r => r.AddAsync(It.IsAny<TbFundo>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_QuandoCodigoDuplicado_DeveLancarDomainException()
    {
        var dto = new CreateFundoRequestDto { Codigo = "EXISTENTE", Nome = "X", Cnpj = "11111111111111", TipoFundoId = 1 };
        _fundoRepoMock.Setup(r => r.ExistsByCodigoAsync("EXISTENTE", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var act = () => _service.CreateAsync(dto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("*código*EXISTENTE*");
    }

    [Fact]
    public async Task CreateAsync_QuandoCnpjDuplicado_DeveLancarDomainException()
    {
        var dto = new CreateFundoRequestDto { Codigo = "NOVO", Nome = "X", Cnpj = "11111111111111", TipoFundoId = 1 };
        _fundoRepoMock.Setup(r => r.ExistsByCodigoAsync("NOVO", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _fundoRepoMock.Setup(r => r.ExistsByCnpjAsync("11111111111111", null, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var act = () => _service.CreateAsync(dto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("*CNPJ*");
    }

    [Fact]
    public async Task CreateAsync_QuandoTipoNaoExiste_DeveLancarDomainException()
    {
        var dto = new CreateFundoRequestDto { Codigo = "NOVO", Nome = "X", Cnpj = "11111111111111", TipoFundoId = 999 };
        _fundoRepoMock.Setup(r => r.ExistsByCodigoAsync("NOVO", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _fundoRepoMock.Setup(r => r.ExistsByCnpjAsync("11111111111111", null, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _tipoFundoRepoMock.Setup(r => r.ExistsFundByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var act = () => _service.CreateAsync(dto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("*Tipo de fundo*999*");
    }

    [Fact]
    public async Task UpdateAsync_QuandoDadosValidos_DeveAtualizarFundo()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Antigo", Cnpj = "11111111111111", TipoFundoId = 1 };
        var dto = new UpdateFundoRequestDto { Nome = "Novo Nome", Cnpj = "11111111111111", TipoFundoId = 2 };

        var callCount = 0;
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                if (callCount == 1) return fundo;
                return new TbFundo { Id = 1, Codigo = "F1", Nome = "Novo Nome", Cnpj = "11111111111111", TipoFundoId = 2, TipoFundo = new TbTipoFundo { Id = 2, Nome = "ACOES" } };
            });
        _fundoRepoMock.Setup(r => r.ExistsByCnpjAsync(dto.Cnpj, "F1", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _tipoFundoRepoMock.Setup(r => r.ExistsFundByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _fundoRepoMock.Setup(r => r.UpdateAsync(It.IsAny<TbFundo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _service.UpdateAsync("F1", dto);

        result.NmFundo.Should().Be("Novo Nome");
        _fundoRepoMock.Verify(r => r.UpdateAsync(It.IsAny<TbFundo>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_QuandoFundoNaoExiste_DeveLancarNotFoundException()
    {
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("INEXISTENTE", It.IsAny<CancellationToken>())).ReturnsAsync((TbFundo?)null);
        var dto = new UpdateFundoRequestDto { Nome = "X", Cnpj = "11111111111111", TipoFundoId = 1 };

        var act = () => _service.UpdateAsync("INEXISTENTE", dto);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateAsync_QuandoCnpjDuplicado_DeveLancarDomainException()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var dto = new UpdateFundoRequestDto { Nome = "Fundo 1", Cnpj = "22222222222222", TipoFundoId = 1 };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>())).ReturnsAsync(fundo);
        _fundoRepoMock.Setup(r => r.ExistsByCnpjAsync("22222222222222", "F1", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var act = () => _service.UpdateAsync("F1", dto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("*CNPJ*");
    }

    [Fact]
    public async Task UpdateAsync_QuandoTipoNaoExiste_DeveLancarDomainException()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var dto = new UpdateFundoRequestDto { Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 999 };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>())).ReturnsAsync(fundo);
        _fundoRepoMock.Setup(r => r.ExistsByCnpjAsync("11111111111111", "F1", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _tipoFundoRepoMock.Setup(r => r.ExistsFundByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var act = () => _service.UpdateAsync("F1", dto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("*Tipo de fundo*999*");
    }

    [Fact]
    public async Task DeleteAsync_QuandoFundoExiste_DeveDeletar()
    {
        _fundoRepoMock.Setup(r => r.ExistsByCodigoAsync("F1", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _fundoRepoMock.Setup(r => r.DeleteAsync("F1", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _service.DeleteAsync("F1");

        _fundoRepoMock.Verify(r => r.DeleteAsync("F1", It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_QuandoFundoNaoExiste_DeveLancarNotFoundException()
    {
        _fundoRepoMock.Setup(r => r.ExistsByCodigoAsync("INEXISTENTE", It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var act = () => _service.DeleteAsync("INEXISTENTE");

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

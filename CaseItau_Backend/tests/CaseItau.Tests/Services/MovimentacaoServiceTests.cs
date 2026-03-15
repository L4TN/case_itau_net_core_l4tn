using AutoMapper;
using CaseItau.Application.DTOs.Movimentacao;
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

public class MovimentacaoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IFundoRepository> _fundoRepoMock;
    private readonly Mock<IMovimentacaoRepository> _movimentacaoRepoMock;
    private readonly Mock<IPosicaoFundoRepository> _posicaoRepoMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<MovimentacaoService>> _loggerMock;
    private readonly MovimentacaoService _service;

    public MovimentacaoServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _fundoRepoMock = new Mock<IFundoRepository>();
        _movimentacaoRepoMock = new Mock<IMovimentacaoRepository>();
        _posicaoRepoMock = new Mock<IPosicaoFundoRepository>();
        _loggerMock = new Mock<ILogger<MovimentacaoService>>();

        _unitOfWorkMock.Setup(u => u.Fundos).Returns(_fundoRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Movimentacoes).Returns(_movimentacaoRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.PosicoesFundo).Returns(_posicaoRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _service = new MovimentacaoService(_unitOfWorkMock.Object, _mapper, _loggerMock.Object);
    }

    [Fact]
    public async Task MovimentarAsync_QuandoFundoNaoExiste_DeveLancarNotFoundException()
    {
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("INEXISTENTE", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbFundo?)null);
        var dto = new CreateMovimentacaoRequestDto { Valor = 1000 };

        var act = () => _service.MovimentarAsync("INEXISTENTE", dto);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task MovimentarAsync_QuandoPrimeiraPosicao_DeveCriarNovaPosicao()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var dto = new CreateMovimentacaoRequestDto { Valor = 5000 };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundo);
        _posicaoRepoMock.Setup(r => r.GetByFundoIdAndDateAsync(1, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbPosicaoFundo?)null);
        _posicaoRepoMock.Setup(r => r.GetLatestByFundoIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbPosicaoFundo?)null);
        _movimentacaoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbMovimentacaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _posicaoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbPosicaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.MovimentarAsync("F1", dto);

        result.VlrPatrimonio.Should().Be(5000);
        _movimentacaoRepoMock.Verify(r => r.AddAsync(It.IsAny<TbMovimentacaoFundo>(), It.IsAny<CancellationToken>()), Times.Once);
        _posicaoRepoMock.Verify(r => r.AddAsync(It.IsAny<TbPosicaoFundo>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MovimentarAsync_QuandoPosicaoExisteNoMesmoDia_DeveAtualizarPosicao()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var posicaoExistente = new TbPosicaoFundo { Id = 1, FundoId = 1, DataPosicao = DateTime.Now, VlrPatrimonio = 10000 };
        var dto = new CreateMovimentacaoRequestDto { Valor = 3000 };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundo);
        _posicaoRepoMock.Setup(r => r.GetByFundoIdAndDateAsync(1, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(posicaoExistente);
        _movimentacaoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbMovimentacaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _posicaoRepoMock.Setup(r => r.UpdateAsync(It.IsAny<TbPosicaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.MovimentarAsync("F1", dto);

        result.VlrPatrimonio.Should().Be(13000);
        _posicaoRepoMock.Verify(r => r.UpdateAsync(It.IsAny<TbPosicaoFundo>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MovimentarAsync_QuandoPosicaoAnteriorExiste_DeveSomarAoPatrimonioAnterior()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var ultimaPosicao = new TbPosicaoFundo { Id = 1, FundoId = 1, DataPosicao = DateTime.Now.AddDays(-1), VlrPatrimonio = 8000 };
        var dto = new CreateMovimentacaoRequestDto { Valor = 2000 };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundo);
        _posicaoRepoMock.Setup(r => r.GetByFundoIdAndDateAsync(1, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbPosicaoFundo?)null);
        _posicaoRepoMock.Setup(r => r.GetLatestByFundoIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ultimaPosicao);
        _movimentacaoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbMovimentacaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _posicaoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbPosicaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.MovimentarAsync("F1", dto);

        result.VlrPatrimonio.Should().Be(10000);
        _posicaoRepoMock.Verify(r => r.AddAsync(It.Is<TbPosicaoFundo>(p => p.VlrPatrimonio == 10000), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MovimentarAsync_QuandoPatrimonioFicariaNegativo_PosicaoExistente_DeveLancarDomainException()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var posicaoExistente = new TbPosicaoFundo { Id = 1, FundoId = 1, DataPosicao = DateTime.Now, VlrPatrimonio = 1000 };
        var dto = new CreateMovimentacaoRequestDto { Valor = -2000 };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundo);
        _posicaoRepoMock.Setup(r => r.GetByFundoIdAndDateAsync(1, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(posicaoExistente);
        _movimentacaoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbMovimentacaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var act = () => _service.MovimentarAsync("F1", dto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("*negativo*");
    }

    [Fact]
    public async Task MovimentarAsync_QuandoPatrimonioFicariaNegativo_NovaPosicao_DeveLancarDomainException()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var ultimaPosicao = new TbPosicaoFundo { Id = 1, FundoId = 1, DataPosicao = DateTime.Now.AddDays(-1), VlrPatrimonio = 500 };
        var dto = new CreateMovimentacaoRequestDto { Valor = -1000 };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundo);
        _posicaoRepoMock.Setup(r => r.GetByFundoIdAndDateAsync(1, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbPosicaoFundo?)null);
        _posicaoRepoMock.Setup(r => r.GetLatestByFundoIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ultimaPosicao);
        _movimentacaoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbMovimentacaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var act = () => _service.MovimentarAsync("F1", dto);

        await act.Should().ThrowAsync<DomainException>().WithMessage("*negativo*");
    }

    [Fact]
    public async Task MovimentarAsync_QuandoResgateParcial_DevePermitir()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var posicaoExistente = new TbPosicaoFundo { Id = 1, FundoId = 1, DataPosicao = DateTime.Now, VlrPatrimonio = 5000 };
        var dto = new CreateMovimentacaoRequestDto { Valor = -3000 };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundo);
        _posicaoRepoMock.Setup(r => r.GetByFundoIdAndDateAsync(1, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(posicaoExistente);
        _movimentacaoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbMovimentacaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _posicaoRepoMock.Setup(r => r.UpdateAsync(It.IsAny<TbPosicaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.MovimentarAsync("F1", dto);

        result.VlrPatrimonio.Should().Be(2000);
    }

    [Fact]
    public async Task MovimentarAsync_DeveGarantirCommitAtomico()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var dto = new CreateMovimentacaoRequestDto { Valor = 1000 };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundo);
        _posicaoRepoMock.Setup(r => r.GetByFundoIdAndDateAsync(1, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbPosicaoFundo?)null);
        _posicaoRepoMock.Setup(r => r.GetLatestByFundoIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbPosicaoFundo?)null);
        _movimentacaoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbMovimentacaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _posicaoRepoMock.Setup(r => r.AddAsync(It.IsAny<TbPosicaoFundo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _service.MovimentarAsync("F1", dto);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetEvolucaoPatrimonialAsync_QuandoFundoExiste_DeveRetornarPosicoes()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var posicoes = new List<TbPosicaoFundo>
        {
            new() { Id = 1, FundoId = 1, DataPosicao = DateTime.Now.AddDays(-2), VlrPatrimonio = 5000 },
            new() { Id = 2, FundoId = 1, DataPosicao = DateTime.Now.AddDays(-1), VlrPatrimonio = 8000 },
            new() { Id = 3, FundoId = 1, DataPosicao = DateTime.Now, VlrPatrimonio = 10000 }
        };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundo);
        _posicaoRepoMock.Setup(r => r.GetByFundoIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(posicoes);

        var result = await _service.GetEvolucaoPatrimonialAsync("F1");

        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetEvolucaoPatrimonialAsync_QuandoFundoNaoExiste_DeveLancarNotFoundException()
    {
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("INEXISTENTE", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbFundo?)null);

        var act = () => _service.GetEvolucaoPatrimonialAsync("INEXISTENTE");

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetMovimentacoesByFundoAsync_QuandoFundoExiste_DeveRetornarMovimentacoes()
    {
        var fundo = new TbFundo { Id = 1, Codigo = "F1", Nome = "Fundo 1", Cnpj = "11111111111111", TipoFundoId = 1 };
        var movimentacoes = new List<TbMovimentacaoFundo>
        {
            new() { Id = 1, FundoId = 1, DataMovimentacao = DateTime.Now.AddDays(-1), VlrMovimentacao = 5000 },
            new() { Id = 2, FundoId = 1, DataMovimentacao = DateTime.Now, VlrMovimentacao = -2000 }
        };

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fundo);
        _movimentacaoRepoMock.Setup(r => r.GetByFundoIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(movimentacoes);

        var result = await _service.GetMovimentacoesByFundoAsync("F1");

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetMovimentacoesByFundoAsync_QuandoFundoNaoExiste_DeveLancarNotFoundException()
    {
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("INEXISTENTE", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbFundo?)null);

        var act = () => _service.GetMovimentacoesByFundoAsync("INEXISTENTE");

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

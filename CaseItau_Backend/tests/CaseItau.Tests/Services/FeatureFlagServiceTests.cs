using AutoMapper;
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

public class FeatureFlagServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IFeatureFlagRepository> _featureFlagRepoMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<FeatureFlagService>> _loggerMock;
    private readonly FeatureFlagService _service;

    public FeatureFlagServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _featureFlagRepoMock = new Mock<IFeatureFlagRepository>();
        _loggerMock = new Mock<ILogger<FeatureFlagService>>();

        _unitOfWorkMock.Setup(u => u.FeatureFlags).Returns(_featureFlagRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _service = new FeatureFlagService(_unitOfWorkMock.Object, _mapper, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodasAsFlags()
    {
        var flags = new List<TbFeatureFlag>
        {
            new() { Id = 1, Chave = "cache_redis", Habilitado = true, Descricao = "Cache Redis" },
            new() { Id = 2, Chave = "nova_feature", Habilitado = false, Descricao = "Nova Feature" }
        };
        _featureFlagRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(flags);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_QuandoNaoHaFlags_DeveRetornarListaVazia()
    {
        _featureFlagRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TbFeatureFlag>());

        var result = await _service.GetAllAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByChaveAsync_QuandoFlagExiste_DeveRetornarFlag()
    {
        var flag = new TbFeatureFlag { Id = 1, Chave = "cache_redis", Habilitado = true, Descricao = "Cache Redis" };
        _featureFlagRepoMock.Setup(r => r.GetByChaveAsync("cache_redis", It.IsAny<CancellationToken>()))
            .ReturnsAsync(flag);

        var result = await _service.GetByChaveAsync("cache_redis");

        result.Should().NotBeNull();
        result!.Chave.Should().Be("cache_redis");
        result.Habilitado.Should().BeTrue();
    }

    [Fact]
    public async Task GetByChaveAsync_QuandoFlagNaoExiste_DeveRetornarNull()
    {
        _featureFlagRepoMock.Setup(r => r.GetByChaveAsync("inexistente", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbFeatureFlag?)null);

        var result = await _service.GetByChaveAsync("inexistente");

        result.Should().BeNull();
    }

    [Fact]
    public async Task IsEnabledAsync_QuandoFlagHabilitada_DeveRetornarTrue()
    {
        _featureFlagRepoMock.Setup(r => r.IsEnabledAsync("cache_redis", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _service.IsEnabledAsync("cache_redis");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsEnabledAsync_QuandoFlagDesabilitada_DeveRetornarFalse()
    {
        _featureFlagRepoMock.Setup(r => r.IsEnabledAsync("cache_redis", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _service.IsEnabledAsync("cache_redis");

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ToggleAsync_QuandoFlagExiste_DeveAlterarEstado()
    {
        var flag = new TbFeatureFlag { Id = 1, Chave = "cache_redis", Habilitado = false, Descricao = "Cache Redis" };
        _featureFlagRepoMock.Setup(r => r.GetByChaveAsync("cache_redis", It.IsAny<CancellationToken>()))
            .ReturnsAsync(flag);
        _featureFlagRepoMock.Setup(r => r.UpdateAsync(It.IsAny<TbFeatureFlag>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.ToggleAsync("cache_redis", true);

        result.Habilitado.Should().BeTrue();
        _featureFlagRepoMock.Verify(r => r.UpdateAsync(It.IsAny<TbFeatureFlag>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ToggleAsync_QuandoFlagNaoExiste_DeveLancarNotFoundException()
    {
        _featureFlagRepoMock.Setup(r => r.GetByChaveAsync("inexistente", It.IsAny<CancellationToken>()))
            .ReturnsAsync((TbFeatureFlag?)null);

        var act = () => _service.ToggleAsync("inexistente", true);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

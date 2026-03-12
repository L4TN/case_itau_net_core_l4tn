using AutoMapper;
using CaseItau.Application.DTOs.Common;
using CaseItau.Application.DTOs.Fundo;
using CaseItau.Application.Services.Interfaces;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CaseItau.Application.Services;

public class FundoService : IFundoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<FundoService> _logger;

    public FundoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FundoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<FundoResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Listando todos os fundos.");
        var fundos = await _unitOfWork.Fundos.GetAllAsync(cancellationToken);
        var dtos = _mapper.Map<IEnumerable<FundoResponseDto>>(fundos).ToList();

        foreach (var dto in dtos)
        {
            var posicao = await _unitOfWork.PosicoesFundo.GetLatestByFundoIdAsync(dto.Id, cancellationToken);
            dto.VlrPatrimonio = posicao?.VlrPatrimonio;
        }

        return dtos;
    }

    public async Task<PagedResult<FundoResponseDto>> GetAllPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Listando fundos paginados. Pagina: {Page}, Tamanho: {PageSize}.", page, pageSize);
        var (items, totalCount) = await _unitOfWork.Fundos.GetAllPagedAsync(page, pageSize, cancellationToken);
        var dtos = _mapper.Map<IEnumerable<FundoResponseDto>>(items).ToList();

        foreach (var dto in dtos)
        {
            var posicao = await _unitOfWork.PosicoesFundo.GetLatestByFundoIdAsync(dto.Id, cancellationToken);
            dto.VlrPatrimonio = posicao?.VlrPatrimonio;
        }

        return new PagedResult<FundoResponseDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<FundoResponseDto> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Buscando fundo com código {Codigo}.", codigo);
        var fundo = await _unitOfWork.Fundos.GetByCodigoAsync(codigo, cancellationToken);
        if (fundo == null)
            throw new NotFoundException(nameof(TbFundo), codigo);

        var dto = _mapper.Map<FundoResponseDto>(fundo);
        var posicao = await _unitOfWork.PosicoesFundo.GetLatestByFundoIdAsync(fundo.Id, cancellationToken);
        dto.VlrPatrimonio = posicao?.VlrPatrimonio;
        return dto;
    }

    public async Task<FundoResponseDto> CreateAsync(CreateFundoRequestDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Criando novo fundo com código {Codigo}.", dto.Codigo);

        if (await _unitOfWork.Fundos.ExistsByCodigoAsync(dto.Codigo, cancellationToken))
            throw new DomainException($"Já existe um fundo com o código '{dto.Codigo}'.");

        if (await _unitOfWork.Fundos.ExistsByCnpjAsync(dto.Cnpj, null, cancellationToken))
            throw new DomainException($"Já existe um fundo com o CNPJ '{dto.Cnpj}'.");

        if (!await _unitOfWork.TiposFundo.ExistsFundByIdAsync(dto.TipoFundoId, cancellationToken))
            throw new DomainException($"Tipo de fundo com id '{dto.TipoFundoId}' não existe.");

        var fundo = _mapper.Map<TbFundo>(dto);
        await _unitOfWork.Fundos.AddAsync(fundo, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        _logger.LogInformation("Fundo {Codigo} criado com sucesso.", fundo.Codigo);

        var fundoCriado = await _unitOfWork.Fundos.GetByCodigoAsync(fundo.Codigo, cancellationToken);
        return _mapper.Map<FundoResponseDto>(fundoCriado);
    }

    public async Task<FundoResponseDto> UpdateAsync(string codigo, UpdateFundoRequestDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Atualizando fundo {Codigo}.", codigo);

        var fundo = await _unitOfWork.Fundos.GetByCodigoAsync(codigo, cancellationToken);
        if (fundo == null)
            throw new NotFoundException(nameof(TbFundo), codigo);

        if (await _unitOfWork.Fundos.ExistsByCnpjAsync(dto.Cnpj, codigo, cancellationToken))
            throw new DomainException($"Já existe outro fundo com o CNPJ '{dto.Cnpj}'.");

        if (!await _unitOfWork.TiposFundo.ExistsFundByIdAsync(dto.TipoFundoId, cancellationToken))
            throw new DomainException($"Tipo de fundo com id '{dto.TipoFundoId}' não existe.");

        _mapper.Map(dto, fundo);
        await _unitOfWork.Fundos.UpdateAsync(fundo, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        _logger.LogInformation("Fundo {Codigo} atualizado com sucesso.", codigo);

        var fundoAtualizado = await _unitOfWork.Fundos.GetByCodigoAsync(codigo, cancellationToken);
        return _mapper.Map<FundoResponseDto>(fundoAtualizado);
    }

    public async Task DeleteAsync(string codigo, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deletando fundo {Codigo}.", codigo);
        if (!await _unitOfWork.Fundos.ExistsByCodigoAsync(codigo, cancellationToken))
            throw new NotFoundException(nameof(TbFundo), codigo);

        await _unitOfWork.Fundos.DeleteAsync(codigo, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        _logger.LogInformation("Fundo {Codigo} deletado com sucesso.", codigo);
    }
}

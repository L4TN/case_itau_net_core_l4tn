using AutoMapper;
using CaseItau.Application.DTOs.Movimentacao;
using CaseItau.Application.Services.Interfaces;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Exceptions;
using CaseItau.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CaseItau.Application.Services;

public class MovimentacaoService : IMovimentacaoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MovimentacaoService> _logger;

    public MovimentacaoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MovimentacaoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PosicaoFundoResponseDto> MovimentarAsync(string codigoFundo, CreateMovimentacaoRequestDto dto, CancellationToken cancellationToken = default)
    {
        var fundo = await _unitOfWork.Fundos.GetByCodigoAsync(codigoFundo, cancellationToken)
            ?? throw new NotFoundException(nameof(TbFundo), codigoFundo);

        var hoje = DateTime.UtcNow.Date;

        _logger.LogInformation("Movimentando fundo {Codigo}. Valor: {Valor}.", codigoFundo, dto.Valor);

        var posicaoHoje = await _unitOfWork.PosicoesFundo.GetByFundoIdAndDateAsync(fundo.Id, hoje, cancellationToken);

        if (posicaoHoje is not null)
        {
            var novoValor = posicaoHoje.VlrPatrimonio + dto.Valor;

            if (novoValor < 0)
                throw new DomainException("Patrimônio não pode ficar negativo.");

            posicaoHoje.VlrPatrimonio = novoValor;
        }
        else
        {
            var ultimaPosicao = await _unitOfWork.PosicoesFundo.GetLatestByFundoIdAsync(fundo.Id, cancellationToken);
            var novoPatrimonio = (ultimaPosicao?.VlrPatrimonio ?? 0) + dto.Valor;

            if (novoPatrimonio < 0)
                throw new DomainException("Patrimônio não pode ficar negativo.");

            posicaoHoje = new TbPosicaoFundo
            {
                FundoId = fundo.Id,
                DataPosicao = hoje,
                VlrPatrimonio = novoPatrimonio
            };
        }

        var movimentacao = new TbMovimentacaoFundo
        {
            FundoId = fundo.Id,
            DataMovimentacao = hoje,
            VlrMovimentacao = dto.Valor
        };
        await _unitOfWork.Movimentacoes.AddAsync(movimentacao, cancellationToken);

        if (posicaoHoje.Id == 0)
            await _unitOfWork.PosicoesFundo.AddAsync(posicaoHoje, cancellationToken);
        else
            await _unitOfWork.PosicoesFundo.UpdateAsync(posicaoHoje, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Fundo {Codigo} movimentado. Patrimônio atual: {Patrimonio}.", codigoFundo, posicaoHoje.VlrPatrimonio);

        return _mapper.Map<PosicaoFundoResponseDto>(posicaoHoje);
    }

    public async Task<IEnumerable<PosicaoFundoResponseDto>> GetEvolucaoPatrimonialAsync(string codigoFundo, CancellationToken cancellationToken = default)
    {
        var fundo = await _unitOfWork.Fundos.GetByCodigoAsync(codigoFundo, cancellationToken)
            ?? throw new NotFoundException(nameof(TbFundo), codigoFundo);

        var posicoes = await _unitOfWork.PosicoesFundo.GetByFundoIdAsync(fundo.Id, cancellationToken);
        return _mapper.Map<IEnumerable<PosicaoFundoResponseDto>>(posicoes);
    }

    public async Task<IEnumerable<MovimentacaoResponseDto>> GetMovimentacoesByFundoAsync(string codigoFundo, CancellationToken cancellationToken = default)
    {
        var fundo = await _unitOfWork.Fundos.GetByCodigoAsync(codigoFundo, cancellationToken)
            ?? throw new NotFoundException(nameof(TbFundo), codigoFundo);

        var movimentacoes = await _unitOfWork.Movimentacoes.GetByFundoIdAsync(fundo.Id, cancellationToken);
        return _mapper.Map<IEnumerable<MovimentacaoResponseDto>>(movimentacoes);
    }
}

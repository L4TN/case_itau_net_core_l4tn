using AutoMapper;
using CaseItau.Application.DTOs.FeatureFlag;
using CaseItau.Application.DTOs.Fundo;
using CaseItau.Application.DTOs.Movimentacao;
using CaseItau.Application.DTOs.TipoFundo;
using CaseItau.Domain.Entities;

namespace CaseItau.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TbFundo, FundoResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CdFundo, opt => opt.MapFrom(src => src.Codigo))
            .ForMember(dest => dest.NmFundo, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.NrCnpj, opt => opt.MapFrom(src => src.Cnpj))
            .ForMember(dest => dest.IdTipoFundo, opt => opt.MapFrom(src => src.TipoFundoId))
            .ForMember(dest => dest.NmTipoFundo, opt => opt.MapFrom(src => src.TipoFundo != null ? src.TipoFundo.Nome : string.Empty))
            .ForMember(dest => dest.VlrPatrimonio, opt => opt.Ignore());

        CreateMap<CreateFundoRequestDto, TbFundo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TipoFundo, opt => opt.Ignore())
            .ForMember(dest => dest.Posicoes, opt => opt.Ignore())
            .ForMember(dest => dest.Movimentacoes, opt => opt.Ignore());

        CreateMap<UpdateFundoRequestDto, TbFundo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Codigo, opt => opt.Ignore())
            .ForMember(dest => dest.TipoFundo, opt => opt.Ignore())
            .ForMember(dest => dest.Posicoes, opt => opt.Ignore())
            .ForMember(dest => dest.Movimentacoes, opt => opt.Ignore());

        CreateMap<TbTipoFundo, TipoFundoResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NmTipoFundo, opt => opt.MapFrom(src => src.Nome));

        CreateMap<TbMovimentacaoFundo, MovimentacaoResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IdFundo, opt => opt.MapFrom(src => src.FundoId))
            .ForMember(dest => dest.DtMovimentacao, opt => opt.MapFrom(src => src.DataMovimentacao))
            .ForMember(dest => dest.VlrMovimentacao, opt => opt.MapFrom(src => src.VlrMovimentacao));

        CreateMap<TbPosicaoFundo, PosicaoFundoResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IdFundo, opt => opt.MapFrom(src => src.FundoId))
            .ForMember(dest => dest.DtPosicao, opt => opt.MapFrom(src => src.DataPosicao))
            .ForMember(dest => dest.VlrPatrimonio, opt => opt.MapFrom(src => src.VlrPatrimonio));

        CreateMap<TbFeatureFlag, FeatureFlagResponseDto>();
    }
}

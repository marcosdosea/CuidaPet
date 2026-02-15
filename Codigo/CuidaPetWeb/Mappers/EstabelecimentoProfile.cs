using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class EstabelecimentoProfile : Profile
    {
        public EstabelecimentoProfile()
        {
            CreateMap<EstabelecimentoViewModel, Estabelecimento>();

            CreateMap<Estabelecimento, EstabelecimentoViewModel>()
                .ForMember(dest => dest.NomeGerente, 
                    opt => opt.MapFrom(src => src.IdGerenteNavigation != null && src.IdGerenteNavigation.IdUsuarioNavigation != null 
                        ? src.IdGerenteNavigation.IdUsuarioNavigation.UserName 
                        : ""));
        }

    }
}

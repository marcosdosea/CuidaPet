using AutoMapper;
using Core;
using Core.DTO;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.IdCategoriaNavigation != null ? src.IdCategoriaNavigation.Nome : string.Empty));

            CreateMap<ProdutoDTO, ProdutoViewModel>()
                .ForMember(dest => dest.IdCategoria, opt => opt.Ignore())
                .ForMember(dest => dest.IdEstabelecimento, opt => opt.Ignore());
        }
    }
}

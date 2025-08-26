using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class EstabelecimentoProfile : Profile
    {
        public EstabelecimentoProfile()
        {
            CreateMap<EstabelecimentoViewModel, Estabelecimento>().ReverseMap();
        }

    }
}

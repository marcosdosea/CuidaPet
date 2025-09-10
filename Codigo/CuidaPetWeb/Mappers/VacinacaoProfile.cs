using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class VacinacaoProfile : Profile
    {
        public VacinacaoProfile()
        {
            CreateMap<VacinacaoViewModel, Vacinacao>().ReverseMap();
        }
    }
}

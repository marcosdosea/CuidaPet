using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class EspecieProfile : Profile
    {
        public EspecieProfile()
        {
            CreateMap<EspecieViewModel, Especie>().ReverseMap();
        }
    }
}

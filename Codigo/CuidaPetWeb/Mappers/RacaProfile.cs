using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class RacaProfile : Profile
    {
        public RacaProfile()
        {
            CreateMap<RacaViewModel, Raca>().ReverseMap();
        }
    }
}

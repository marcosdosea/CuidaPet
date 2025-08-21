using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class DoencaProfile : Profile
    {
        public DoencaProfile()
        {
            CreateMap<DoencaViewModel, Doenca>().ReverseMap();
        }
    }
}

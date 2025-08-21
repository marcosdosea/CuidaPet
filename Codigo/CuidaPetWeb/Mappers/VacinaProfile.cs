using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class VacinaProfile : Profile
    {
        public VacinaProfile()
        {
            CreateMap<VacinaViewModel, Vacina>().ReverseMap();
        }

    }
}

using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class EspecialidadeProfile : Profile
    {
        public EspecialidadeProfile()
        {
            CreateMap<EspecialidadeViewModel, Especialidade>().ReverseMap();
        }
    }
}

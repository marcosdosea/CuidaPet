using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class PetProfile : Profile
    {
        public PetProfile()
        {
            CreateMap<PetViewModel, Pet>().ReverseMap();
        }
    }
}

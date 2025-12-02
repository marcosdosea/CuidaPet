using AutoMapper;
using Core.DTO;

namespace CuidaPetWeb.Mappers
{
    public class ConsultarPetProfile : Profile
    {
        public ConsultarPetProfile()
        {
            CreateMap<ConsultarPetDto, ConsultarPetDto>();
        }
    }
}
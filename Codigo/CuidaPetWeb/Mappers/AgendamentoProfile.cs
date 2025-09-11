using AutoMapper;
using Core;
using Core.DTO;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class AgendamentoProfile : Profile
    {
        public AgendamentoProfile()
        {
            CreateMap<AgendamentoViewModel, AgendamentoDto>().ReverseMap();
            CreateMap<AgendamentoViewModel, Agendamento>().ReverseMap();
        } 
    }
}

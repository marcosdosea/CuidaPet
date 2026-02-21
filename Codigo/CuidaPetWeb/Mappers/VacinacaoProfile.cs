using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class VacinacaoProfile : Profile
    {
        public VacinacaoProfile()
        {
            CreateMap<Vacinacao, VacinacaoViewModel>()
                .ForMember(dest => dest.NomeVacina, opt => opt.MapFrom(src => src.IdVacinaNavigation.Nome))
                .ForMember(dest => dest.NomePet, opt => opt.MapFrom(src => src.IdPetNavigation.Nome))
                .ForMember(dest => dest.NomeFuncionario, opt => opt.MapFrom(src => src.IdFuncionarioNavigation.IdPessoaNavigation.IdUsuarioNavigation.UserName));

            CreateMap<VacinacaoViewModel, Vacinacao>();
        }
    }
}
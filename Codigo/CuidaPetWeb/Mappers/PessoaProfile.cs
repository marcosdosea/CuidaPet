using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class PessoaProfile : Profile
    {
        public PessoaProfile()
        {
            CreateMap<Pessoa, PessoaViewModel>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.IdUsuarioNavigation.UserName ?? ""))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.IdUsuarioNavigation.Email ?? ""))
                .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.IdUsuarioNavigation.PhoneNumber ?? ""))
                .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => ""));

            CreateMap<PessoaViewModel, Pessoa>();

            CreateMap<FuncionarioViewModel, Pessoa>().ReverseMap();
        }
    }
}

using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers;

public class FuncionarioProfile : Profile
{
    public FuncionarioProfile()
    {
        CreateMap<Funcionario, FuncionarioViewModel>()
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.IdPessoaNavigation.IdUsuarioNavigation.UserName ?? ""))
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.IdPessoaNavigation.Cpf))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.IdPessoaNavigation.IdUsuarioNavigation.Email ?? ""))
            .ForMember(dest => dest.Senha, opt => opt.MapFrom(src => "")) // Senha não deve ser exposta
            .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.IdPessoaNavigation.IdUsuarioNavigation.PhoneNumber ?? ""))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => "V")) // Tipo fixo ou buscar da role
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IdPessoaNavigation.Status))
            .ForMember(dest => dest.Logradouro, opt => opt.MapFrom(src => src.IdPessoaNavigation.Logradouro))
            .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.IdPessoaNavigation.Numero))
            .ForMember(dest => dest.Complemento, opt => opt.MapFrom(src => src.IdPessoaNavigation.Complemento))
            .ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => src.IdPessoaNavigation.Bairro))
            .ForMember(dest => dest.Cidade, opt => opt.MapFrom(src => src.IdPessoaNavigation.Cidade))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.IdPessoaNavigation.Estado));

        CreateMap<FuncionarioViewModel, Funcionario>();


    }
}

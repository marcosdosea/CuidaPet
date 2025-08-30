using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers;

public class FuncionarioProfile : Profile
{
    public FuncionarioProfile()
    {
        CreateMap<Funcionario, FuncionarioViewModel>().ReverseMap();
    }
}

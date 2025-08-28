using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class PessoaProfile : Profile
    {
        public PessoaProfile()
        {            
            CreateMap<PessoaViewModel, Pessoa>().ReverseMap();
        }
    }
}

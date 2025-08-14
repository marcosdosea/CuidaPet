using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            CreateMap<ProdutoViewModel, Produto>().ReverseMap();
        }
    }
}

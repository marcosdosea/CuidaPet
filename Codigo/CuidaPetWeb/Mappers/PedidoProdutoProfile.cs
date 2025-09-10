using AutoMapper;
using Core.DTO;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class PedidoProdutoProfile : Profile
    {
        public PedidoProdutoProfile()
        {
            CreateMap<PedidoProdutoDto, PedidoProdutoViewModel>().ReverseMap();
        }
    }
}

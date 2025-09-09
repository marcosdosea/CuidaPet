using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class PedidoProdutoProfile : Profile
    {
        public PedidoProdutoProfile()
        {
            CreateMap<PedidoProdutoViewModel, Pedidoproduto>().ReverseMap();
        }
    }
}

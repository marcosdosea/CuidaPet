using AutoMapper;
using Core;
using Core.DTO;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class ConsultarEstabelecimentosProfile : Profile
    {
        public ConsultarEstabelecimentosProfile()
        {
            CreateMap<Estabelecimento, ConsultarEstabelecimentosViewModel>();
            CreateMap<Estabelecimento, DetalhesEstabelecimentoViewModel>();
            CreateMap<Produto, ProdutoViewModel>();
            CreateMap<ProdutoDTO, ProdutoViewModel>();
            CreateMap<Estabelecimento, EstabelecimentoComProdutosViewModel>();
        }
    }
}

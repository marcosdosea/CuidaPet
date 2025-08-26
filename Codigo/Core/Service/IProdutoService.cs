using Core.DTO;

namespace Core.Service
{
    public interface IProdutoService
    {
        uint Create(Produto produto);
        void Edit(Produto produto);
        void Delete(uint id);
        Produto? Get(uint id);
        IEnumerable<Produto> GetAll(int page, int pageSize);
        IEnumerable<ProdutoDTO> GetByNome(string nome);
        IEnumerable<ProdutoDTO> GetByEstabelecimento(uint idEstabelecimento);
        IEnumerable<ProdutoDTO> GetByNomeAndEstabelecimento(string nome, uint idEstabelecimento);
        IEnumerable<ProdutoDTO> GetByCategoria(uint idCategoria);
        IEnumerable<ProdutoDTO> GetProdutosPromocao();
        int getCount();
    }
}

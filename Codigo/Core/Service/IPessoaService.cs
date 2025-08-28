using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IPessoaService
    {
        uint Create(Pessoa pessoa);
        void Edit(Pessoa pessoa);
        void Delete(uint id);
        Pessoa? Get(uint id);
        IEnumerable<Pessoa> GetAll(int page, int pageSize);
        int GetCount();
        // TODO: CPF
        // string GetCpf(uint idPessoa);
    }
}

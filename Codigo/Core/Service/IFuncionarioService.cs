using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service;

public interface IFuncionarioService
{
    uint Create(Funcionario funcionario);
    void Edit(Funcionario funcionario);
    void Delete(uint id);
    Funcionario? Get(uint id);
    IEnumerable<Funcionario> GetAll(int page, int pageSize);
    int GetCount();
}

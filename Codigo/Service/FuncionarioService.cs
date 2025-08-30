using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service;

public class FuncionarioService : IFuncionarioService
{
    private readonly CuidaPetContext context;

    public FuncionarioService(CuidaPetContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Criar um novo funcionário na base de dados
    /// </summary>
    /// <param name="funcionario">Dados do Funcionário</param>
    /// <returns>ID do Funcionário</returns>
    public uint Create(Funcionario funcionario)
    {
        context.Funcionarios.Add(funcionario);
        context.SaveChanges();
        return funcionario.Id;
    }

    /// <summary>
    /// Editar um funcionário existente na base de dados
    /// </summary>
    /// <param name="funcionario">Dados do Funcionário</param>
    public void Edit(Funcionario funcionario)
    {
        context.Funcionarios.Update(funcionario);
        context.SaveChanges();
    }

    /// <summary>
    /// Deletar um funcionário da base de dados
    /// </summary>
    /// <param name="id">ID do Funcionário</param>
    public void Delete(uint id)
    {
        var funcionario = context.Funcionarios.Find(id);
        if (funcionario != null)
        {
            context.Funcionarios.Remove(funcionario);
            context.SaveChanges();
        }
    }

    /// <summary>
    /// Buscar um funcionário na base de dados
    /// </summary>
    /// <param name="id">ID do Funcionário</param>
    /// <returns>Dados do Funcionário</returns>
    public Funcionario? Get(uint id)
    {
        return context.Funcionarios
            .Include(f => f.IdPessoaNavigation)
            .Include(f => f.IdEstabelecimentoNavigation)
            .FirstOrDefault(f => f.Id == id);
    }

    /// <summary>
    /// Buscar todos os funcionários na base de dados
    /// </summary>
    /// <returns>Lista de Funcionários</returns>
    public IEnumerable<Funcionario> GetAll(int page = 1, int pageSize = 10)
    {
        return context.Funcionarios
                .Include(f => f.IdEstabelecimentoNavigation)
                .Include(f => f.IdPessoaNavigation)
                .AsNoTracking()
                .OrderBy(f => f.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
    }

    /// <summary>
    /// Obter contagem total de funcionários
    /// </summary>
    /// <returns>Número total de funcionários</returns>
    public int GetCount()
    {
        return context.Funcionarios.Count();
    }
}
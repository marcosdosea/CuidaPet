using Core.DTO;

namespace Core.Service
{
    public interface IConsultarPetService
    {
        /// <summary>
        /// Obtém as informações completas do pet para consulta veterinária
        /// </summary>
        /// <param name="idPet">ID do Pet</param>
        /// <param name="idAgendamento">ID do Agendamento</param>
        /// <returns>Dados completos do pet para consulta</returns>
        ConsultarPetDto? ObterDadosPetParaConsulta(uint idPet, uint idAgendamento);
        
        /// <summary>
        /// Finaliza a consulta registrando observações e criando o registro de consulta
        /// </summary>
        /// <param name="consultarPetDto">Dados da consulta</param>
        /// <returns>ID da consulta criada</returns>
        uint FinalizarConsulta(ConsultarPetDto consultarPetDto);
        
        /// <summary>
        /// Obtém a lista de agendamentos aprovados (status 'A') para o veterinário
        /// </summary>
        /// <param name="idFuncionario">ID do Funcionário (Veterinário)</param>
        /// <returns>Lista de agendamentos aprovados</returns>
        IEnumerable<AgendamentoConsultaDto> ObterAgendamentosAprovados(uint idFuncionario);
    }
}
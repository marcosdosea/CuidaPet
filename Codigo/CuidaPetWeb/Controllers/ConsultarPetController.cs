using Core.DTO;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultarPetController : ControllerBase
    {
        private readonly IConsultarPetService consultarPetService;

        public ConsultarPetController(IConsultarPetService consultarPetService)
        {
            this.consultarPetService = consultarPetService;
        }

        /// <summary>
        /// Obtém a lista de agendamentos aprovados para o veterinário
        /// </summary>
        /// <param name="idFuncionario">ID do Funcionário (Veterinário)</param>
        /// <returns>Lista de agendamentos</returns>
        [HttpGet("agendamentos-aprovados/{idFuncionario}")]
        public ActionResult<IEnumerable<AgendamentoConsultaDto>> ObterAgendamentosAprovados(uint idFuncionario)
        {
            try
            {
                var agendamentos = consultarPetService.ObterAgendamentosAprovados(idFuncionario);
                return Ok(agendamentos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtém os dados completos do pet para consulta
        /// </summary>
        /// <param name="idPet">ID do Pet</param>
        /// <param name="idAgendamento">ID do Agendamento</param>
        /// <returns>Dados do pet para consulta</returns>
        [HttpGet("dados-pet/{idPet}/agendamento/{idAgendamento}")]
        public ActionResult<ConsultarPetDto> ObterDadosPetParaConsulta(uint idPet, uint idAgendamento)
        {
            try
            {
                var dados = consultarPetService.ObterDadosPetParaConsulta(idPet, idAgendamento);
                
                if (dados == null)
                    return NotFound(new { message = "Pet ou agendamento não encontrado, ou agendamento não está aprovado." });

                return Ok(dados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Finaliza a consulta registrando as observações
        /// </summary>
        /// <param name="consultarPetDto">Dados da consulta</param>
        /// <returns>ID da consulta criada</returns>
        [HttpPost("finalizar")]
        public ActionResult<uint> FinalizarConsulta([FromBody] ConsultarPetDto consultarPetDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var idConsulta = consultarPetService.FinalizarConsulta(consultarPetDto);
                return Ok(new { idConsulta, message = "Consulta finalizada com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
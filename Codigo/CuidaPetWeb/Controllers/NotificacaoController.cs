using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    public class NotificacaoController : Controller
    {
        private readonly INotificacaoService notificacaoService;
        private readonly IMapper mapper;

        public NotificacaoController(INotificacaoService notificacaoService, IMapper mapper)
        {
            this.notificacaoService = notificacaoService;
            this.mapper = mapper;
        }

        // GET: NotificacaoController - Página de notificações do usuário
        public IActionResult Index()
        {

            // TODO: Obter ID do usuário logado via Identity
            const uint TESTE_ID_PESSOA = 1;

            var notificacoesDto = notificacaoService.ObterNotificacoesComStatusPorPessoa(TESTE_ID_PESSOA);

            var viewModel = notificacoesDto.Select(dto => new NotificacaoViewModel
            {
                Id = dto.Id,
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                DataEnvio = dto.DataEnvio,
                StatusLida = (sbyte)(dto.Lida ? 1 : 0)
            }).ToList();

            ViewBag.TotalNotificacoes = viewModel.Count;
            ViewBag.NotificacoesNaoLidas = viewModel.Count(n => !n.EstaLida);

            return View(viewModel);


        }

        // GET: NotificacaoController/Details/5
        public IActionResult Details(uint id)
        {
            var notificacao = notificacaoService.Get(id);

            var viewModel = mapper.Map<NotificacaoViewModel>(notificacao);
            return View(viewModel);
        }

        // GET: NotificacaoController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotificacaoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NotificacaoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var notificacao = mapper.Map<Notificacao>(viewModel);
                notificacao.DataEnvio = DateTime.Now;
                notificacaoService.Create(notificacao);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: NotificacaoController/Edit/5
        public IActionResult Edit(uint id)
        {
            var notificacao = notificacaoService.Get(id);

            var viewModel = mapper.Map<NotificacaoViewModel>(notificacao);
            return View(viewModel);
        }

        // POST: NotificacaoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(uint id, NotificacaoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = mapper.Map<Notificacao>(viewModel);
                notificacaoService.Edit(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: NotificacaoController/Delete/5
        public IActionResult Delete(uint id)
        {
            var notificacao = notificacaoService.Get(id);

            var viewModel = mapper.Map<NotificacaoViewModel>(notificacao);
            return View(viewModel);
        }

        // POST: NotificacaoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(uint id, NotificacaoViewModel notificacaoViewModel)
        {
            notificacaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Marcar notificação como lida
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarcarComoLida(uint idNotificacao)
        {
            // TODO: Obter ID do usuário logado via Identity
            const uint TESTE_ID_PESSOA = 1;

            notificacaoService.MarcarComoLida(idNotificacao, TESTE_ID_PESSOA);
            return RedirectToAction(nameof(Index));
        }

        // API endpoint para obter contagem de notificações não lidas
        [HttpGet]
        public IActionResult GetContagemNaoLidas()
        {
            try
            {
                // TODO: Obter ID do usuário logado via Identity
                const uint TESTE_ID_PESSOA = 1;

                var contagem = notificacaoService.ObterContagemNaoLidas(TESTE_ID_PESSOA);
                return Json(new { success = true, count = contagem });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
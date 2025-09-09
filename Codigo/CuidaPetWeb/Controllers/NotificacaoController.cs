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

        // GET: NotificacaoController
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var notificacoes = notificacaoService.GetAll(page, pageSize);
            var viewModel = mapper.Map<IEnumerable<NotificacaoViewModel>>(notificacoes);

            ViewBag.TotalCount = notificacaoService.GetCount();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

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
                notificacao.DataEnvio = DateTime.Now; // Define a data atual
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

        // Método específico para listar notificações de um usuário
        public IActionResult MinhasNotificacoes(uint idPessoa)
        {
            var notificacoes = notificacaoService.ObterNotificacoesPorPessoa(idPessoa);
            var viewModel = mapper.Map<IEnumerable<NotificacaoViewModel>>(notificacoes);
            return View(viewModel);
        }

        // Método para marcar notificação como lida
        [HttpPost]
        public IActionResult MarcarComoLida(uint idNotificacao, uint idPessoa)
        {
            notificacaoService.MarcarComoLida(idNotificacao, idPessoa);
            return RedirectToAction(nameof(MinhasNotificacoes), new { idPessoa });
        }
    }
}

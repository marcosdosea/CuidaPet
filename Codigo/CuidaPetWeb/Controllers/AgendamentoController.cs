using AutoMapper;
using Core;
using Core.Service;
using CuidaPetWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CuidaPetWeb.Controllers
{
    [Authorize]
    public class AgendamentoController : Controller
    {
        private readonly IAgendamentoService agendamentoService;
        private readonly IMapper mapper;

        public AgendamentoController(IAgendamentoService agendamentoService, IMapper mapper)
        {
            this.agendamentoService = agendamentoService;
            this.mapper = mapper;
        }

        // GET: Agendamento
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var agendamentos = agendamentoService.GetAll(page, pageSize);
            var agendamentoViewModels = mapper.Map<IEnumerable<AgendamentoViewModel>>(agendamentos);

            ViewBag.TotalItems = agendamentoService.GetCount();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            return View(agendamentoViewModels);
        }

        // GET: Agendamento/Details/5
        public ActionResult Details(uint id)
        {
            var agendamento = agendamentoService.Get(id);

            var agendamentoViewModel = mapper.Map<AgendamentoViewModel>(agendamento);
            return View(agendamentoViewModel);
        }

        // GET: Agendamento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agendamento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AgendamentoViewModel agendamentoViewModel)
        {
            if (ModelState.IsValid)
            {
                var agendamento = mapper.Map<Agendamento>(agendamentoViewModel);
                agendamentoService.Create(agendamento);
                return RedirectToAction(nameof(Index));
            }
            return View(agendamentoViewModel);
        }

        // GET: Agendamento/Edit/5
        public ActionResult Edit(uint id)
        {
            var agendamento = agendamentoService.Get(id);
            var agendamentoViewModel = mapper.Map<AgendamentoViewModel>(agendamento);
            return View(agendamentoViewModel);
        }

        // POST: Agendamento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AgendamentoViewModel agendamentoViewModel)
        {
            if (ModelState.IsValid)
            {
                var agendamento = mapper.Map<Agendamento>(agendamentoViewModel);
                agendamentoService.Edit(agendamento);
                return RedirectToAction(nameof(Index));
            }
            return View(agendamentoViewModel);
        }

        // GET: Agendamento/Delete/5
        public ActionResult Delete(uint id)
        {
            var agendamento = agendamentoService.Get(id);
            var agendamentoViewModel = mapper.Map<AgendamentoViewModel>(agendamento);
            return View(agendamentoViewModel);
        }

        // POST: Agendamento/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, AgendamentoViewModel agendamentoViewModel)
        {
            agendamentoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

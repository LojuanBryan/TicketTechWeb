using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using testeTicketTech.Data;
using testeTicketTech.Filters;
using testeTicketTech.Models;
using testeTicketTech.Helper;
using Microsoft.EntityFrameworkCore;

namespace testeTicketTech.Controllers
{
    [PaginaParaUsuarioLogado]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly ISessao _sessao;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, ISessao sessao)
        {
            _logger = logger;
            _db = db;
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

            // Estatísticas gerais
            var totalChamados = _db.Chamados.Count();
            var chamadosAbertos = _db.Chamados.Count(c => c.Status == "Aberto");
            var chamadosEmAndamento = _db.Chamados.Count(c => c.Status == "Em Andamento");
            var chamadosResolvidos = _db.Chamados.Count(c => c.Status == "Resolvido");

            // Chamados do usuário (se não for admin)
            var meusChamados = _db.Chamados.Count(c => c.UsuarioId == usuarioLogado.Id);
            var meusChamadosAbertos = _db.Chamados.Count(c => c.UsuarioId == usuarioLogado.Id && c.Status == "Aberto");

            List<Chamados> ultimosChamados;

            if (usuarioLogado.Perfil == Enums.PerfilEnum.Admin)
            {
                // Admin vê todos os chamados
                ultimosChamados = _db.Chamados
                    .OrderByDescending(c => c.DataUltimaAtualizacao)
                    .Take(5)
                    .ToList();
            }
            else
            {
                // Usuário padrão vê apenas os seus
                ultimosChamados = _db.Chamados
                    .Where(c => c.UsuarioId == usuarioLogado.Id)
                    .OrderByDescending(c => c.DataUltimaAtualizacao)
                    .Take(5)
                    .ToList();
            }


            // Total de usuários (apenas para admin)
            var totalUsuarios = _db.Usuarios.Count();

            ViewBag.TotalChamados = totalChamados;
            ViewBag.ChamadosAbertos = chamadosAbertos;
            ViewBag.ChamadosEmAndamento = chamadosEmAndamento;
            ViewBag.ChamadosResolvidos = chamadosResolvidos;
            ViewBag.MeusChamados = meusChamados;
            ViewBag.MeusChamadosAbertos = meusChamadosAbertos;
            ViewBag.UltimosChamados = ultimosChamados;
            ViewBag.TotalUsuarios = totalUsuarios;
            ViewBag.UsuarioLogado = usuarioLogado;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
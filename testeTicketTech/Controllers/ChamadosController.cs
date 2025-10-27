using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using testeTicketTech.Data;
using testeTicketTech.Helper;
using testeTicketTech.Models;
using testeTicketTech.Repositorios;

namespace testeTicketTech.Controllers
{
    public class ChamadosController : Controller
    {
        private readonly IChamadoRepositorio _chamadoRepositorio;
        private readonly ISessao _sessao;


        public ChamadosController(IChamadoRepositorio chamadoRepositorio, ISessao sessao)
        {
            _chamadoRepositorio = chamadoRepositorio;
            _sessao = sessao;
        }

        // 🔹 Lista chamados com base no perfil
        public IActionResult Index()
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

            List<Chamados> chamados;

            if (usuarioLogado.Perfil == Enums.PerfilEnum.Admin)
            {
                chamados = _chamadoRepositorio.BuscarTodos(); // Admin vê todos
            }
            else
            {
                chamados = _chamadoRepositorio.BuscarTodosComUsuarios(usuarioLogado.Id); // Padrão vê só os seus
            }

            return View(chamados);
        }
        // Exibe a página para alterar o status (apenas Admin)
        [HttpGet]
        [PaginaRestritaSomenteAdmin]
        public IActionResult AlterarStatus(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var chamado = _db.Chamados.FirstOrDefault(x => x.ChamadoId == id);

            if (chamado == null)
                return NotFound();

            return View(chamado);
        }

        // Processa a alteração do status (apenas Admin)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PaginaRestritaSomenteAdmin]
        public IActionResult AlterarStatus(int id, string novoStatus)
        {
            var chamado = _db.Chamados.FirstOrDefault(c => c.ChamadoId == id);

            if (chamado == null)
            {
                TempData["MensagemErro"] = "Chamado não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            // Atualizar o status
            chamado.Status = novoStatus;
            chamado.DataUltimaAtualizacao = DateTime.Now;

            _db.SaveChanges();

            TempData["MensagemSucesso"] = $"Status alterado para '{novoStatus}' com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        // Exibe os detalhes completos do chamado
        [HttpGet]
        public IActionResult Visualizar(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var chamado = _chamadoRepositorio.BuscarPorId(id.Value);
            if (chamado == null)
                return NotFound();

            return View(chamado);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(Chamados chamado)
        {
            if (ModelState.IsValid)
            {
                var usuario = _sessao.BuscarSessaoDoUsuario();
                chamado.UsuarioId = usuario.Id;
                chamado.DataUltimaAtualizacao = DateTime.Now;
                chamado.Status = "Aberto";

                _chamadoRepositorio.Adicionar(chamado);

                TempData["MensagemSucesso"] = "Chamado realizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(chamado);
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var chamado = _chamadoRepositorio.BuscarPorId(id.Value);
            if (chamado == null)
                return NotFound();

            return View(chamado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Chamados chamado)
        {
            if (ModelState.IsValid)
            {
                chamado.DataUltimaAtualizacao = DateTime.Now;
                _chamadoRepositorio.Atualizar(chamado);

                TempData["MensagemSucesso"] = "Edição realizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            TempData["MensagemErro"] = "Ocorreu um erro ao realizar a edição";
            return View(chamado);
        }

        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var chamado = _chamadoRepositorio.BuscarPorId(id.Value);
            if (chamado == null)
                return NotFound();

            return View(chamado);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public IActionResult ExcluirConfirmado(int id)
        {
            _chamadoRepositorio.Remover(id);

            TempData["MensagemSucesso"] = "Remoção realizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Exportar()
        {
            DataTable dados = GetDados();

            using (var workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(dados, "Dados Chamados");

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return File(
                        ms.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Chamados.xlsx"
                    );
                }
            }
        }

        private DataTable GetDados()
        {
            DataTable datatable = new DataTable();
            datatable.TableName = "Chamados Export";
            datatable.Columns.Add("ID Chamado", typeof(int));
            datatable.Columns.Add("Usuario", typeof(string));
            datatable.Columns.Add("Titulo", typeof(string));
            datatable.Columns.Add("Dispositivo", typeof(string));
            datatable.Columns.Add("Descrição", typeof(string));
            datatable.Columns.Add("Status", typeof(string));
            datatable.Columns.Add("Ultima Atualizacao", typeof(DateTime));

            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            var dados = usuarioLogado.Perfil == Enums.PerfilEnum.Admin
                ? _chamadoRepositorio.BuscarTodos()
                : _chamadoRepositorio.BuscarTodosComUsuarios(usuarioLogado.Id);

            foreach (var chamado in dados)
            {
                datatable.Rows.Add(
                    chamado.ChamadoId,
                    chamado.Usuario?.Nome ?? "Desconhecido",
                    chamado.Titulo,
                    chamado.Dispositivo,
                    chamado.DescricaoDetalhada,
                    chamado.Status,
                    chamado.DataUltimaAtualizacao
                );
            }

            return datatable;
        }
    }
}

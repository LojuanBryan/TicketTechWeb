using Microsoft.AspNetCore.Mvc;
using testeTicketTech.Data;
using testeTicketTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace testeTicketTech.Controllers
{
    
    public class ChamadosController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ChamadosController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Exibe a lista de todos os chamados
        public IActionResult Index()
        {
            IEnumerable<Chamados> chamados = _db.Chamados.ToList();
            return View(chamados);
        }

        // Exibe o formulário para criar um novo chamado
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        // Recebe os dados do formulário e salva no banco
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(Chamados chamado)
        {
            if (ModelState.IsValid)
            {
                chamado.DataUltimaAtualizacao = DateTime.Now;
                chamado.Status = "Aberto"; // opcional, já vem do model
                _db.Chamados.Add(chamado);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Chamado realizado com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            // Se houver erro de validação, retorna a mesma view com os dados preenchidos
            return View(chamado);
        }

        // Exibe o formulário para editar um chamado
        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var chamado = _db.Chamados.FirstOrDefault(x => x.ChamadoId == id);
            if (chamado == null)
                return NotFound();

            return View(chamado);
        }

        // Recebe os dados editados e salva as alterações
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Chamados chamado)
        {
            if (ModelState.IsValid)
            {
                var chamadoDoBanco = _db.Chamados.FirstOrDefault(c => c.ChamadoId == chamado.ChamadoId);
                if (chamadoDoBanco == null)
                    return NotFound();

                // Atualizar campos
                chamadoDoBanco.Titulo = chamado.Titulo;
                chamadoDoBanco.Dispositivo = chamado.Dispositivo;
                chamadoDoBanco.Sintomas = chamado.Sintomas;
                chamadoDoBanco.QuandoOcorreu = chamado.QuandoOcorreu;
                chamadoDoBanco.OndeOcorreu = chamado.OndeOcorreu;
                chamadoDoBanco.DescricaoDetalhada = chamado.DescricaoDetalhada;
                chamadoDoBanco.Status = chamado.Status;
                chamadoDoBanco.DataUltimaAtualizacao = DateTime.Now;

                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Edição realizada com sucesso!";

                return RedirectToAction(nameof(Index));

            }

            TempData["MensagemErro"] = "Ocorreu um erro ao realizar a edição";


            // Se houver erro de validação, retorna a mesma view com os dados preenchidos
            return View(chamado);
        }


        // Exibe confirmação de exclusão
        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var chamado = _db.Chamados.FirstOrDefault(x => x.ChamadoId == id);
            if (chamado == null)
                return NotFound();

            return View(chamado);
        }

        // Remove o chamado do banco
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public IActionResult ExcluirConfirmado(int id)
        {
            var chamado = _db.Chamados.FirstOrDefault(c => c.ChamadoId == id);
            if (chamado == null)
                return NotFound();

            _db.Chamados.Remove(chamado);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Remoção realizada com sucesso!";

            return RedirectToAction(nameof(Index));
        }

    }
}

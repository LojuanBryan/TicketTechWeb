using Microsoft.AspNetCore.Mvc;
using testeTicketTech.Data;
using testeTicketTech.Models;

namespace testeTicketTech.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UsuarioController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var usuarios = _db.Usuarios.ToList();
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                _db.Usuarios.Add(usuario);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }


        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuario = _db.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                TempData["MensagemErro"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(UsuarioModel usuario)
        {
            var usuarioDb = _db.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            if (usuarioDb == null)
            {
                TempData["MensagemErro"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                usuarioDb.Nome = usuario.Nome;
                usuarioDb.Login = usuario.Login;
                usuarioDb.Email = usuario.Email;
                usuarioDb.Endereco = usuario.Endereco;
                usuarioDb.Perfil = usuario.Perfil;
                usuarioDb.Senha = usuario.Senha;
                usuarioDb.DataAtualizacao = DateTime.Now;

                _db.Usuarios.Update(usuarioDb);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Usuário atualizado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(usuario);
        }



        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var usuario = _db.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                TempData["MensagemErro"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarExclusao(int id)
        {
            var usuario = _db.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                TempData["MensagemErro"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }

            _db.Usuarios.Remove(usuario);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Usuário excluído com sucesso!";
            return RedirectToAction("Index");
        }


    }

}
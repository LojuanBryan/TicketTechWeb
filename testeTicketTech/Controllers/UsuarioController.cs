using Microsoft.AspNetCore.Mvc;
using System.Text;
using testeTicketTech.Data;
using testeTicketTech.Filters;
using testeTicketTech.Models;
using System.Security.Cryptography;


namespace testeTicketTech.Controllers
{
    [PaginaRestritaSomenteAdmin]

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

        private string Criptografar(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
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
                usuario.Senha = Criptografar(usuario.Senha); // ✅ Criptografa antes de salvar
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
                usuarioDb.DataAtualizacao = DateTime.Now;

                // 🔹 Se o campo senha foi preenchido, criptografa antes de salvar
                if (!string.IsNullOrEmpty(usuario.Senha))
                {
                    usuarioDb.Senha = Criptografar(usuario.Senha);
                }

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
        // Exibe os detalhes completos do usuário
        [HttpGet]
        public IActionResult Visualizar(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["MensagemErro"] = "ID inválido.";
                return RedirectToAction("Index");
            }

            var usuario = _db.Usuarios.FirstOrDefault(u => u.Id == id);

            if (usuario == null)
            {
                TempData["MensagemErro"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }

            // Buscar chamados do usuário
            var chamadosDoUsuario = _db.Chamados.Where(c => c.UsuarioId == id).ToList();
            ViewBag.ChamadosDoUsuario = chamadosDoUsuario;
            ViewBag.TotalChamados = chamadosDoUsuario.Count;
            ViewBag.ChamadosAbertos = chamadosDoUsuario.Count(c => c.Status == "Aberto");
            ViewBag.ChamadosResolvidos = chamadosDoUsuario.Count(c => c.Status == "Resolvido");

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
using Microsoft.AspNetCore.Mvc;
using testeTicketTech.Data;
using testeTicketTech.Helper;
using testeTicketTech.Models;

namespace testeTicketTech.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ISessao _sessao;

        public LoginController(ApplicationDbContext db, ISessao sessao)
        {
            _db = db;
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            //Se o usuario estiver logado, redirecionar para a home

            if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");

            return View();
        }

        //Método para deslogar

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Index", loginModel);

                // Verifica se o usuário existe no banco
                var usuario = _db.Usuarios.FirstOrDefault(u =>
                    u.Login == loginModel.Login &&
                    u.Senha == loginModel.Senha);

                if (usuario == null)
                {
                    TempData["MensagemErro"] = "Usuário ou senha inválidos. Verifique seus dados ou cadastre-se no sistema.";
                    return RedirectToAction("Index");
                }


                if (!loginModel.ConcordaLGPD)
                {
                    ModelState.AddModelError("ConcordaLGPD", "Você deve concordar com a LGPD para continuar.");
                    return View("Index", loginModel);
                }

                _sessao.CriarSessaoDoUsuario(usuario);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao fazer login: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}

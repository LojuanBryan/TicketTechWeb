using Microsoft.AspNetCore.Mvc;
using testeTicketTech.Data;
using testeTicketTech.Models;

namespace testeTicketTech.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;

        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
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

                // Aqui você pode salvar o usuário na sessão, se quiser
                TempData["MensagemSucesso"] = $"Bem-vindo, {usuario.Nome}!";
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

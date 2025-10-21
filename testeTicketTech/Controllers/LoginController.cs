using Microsoft.AspNetCore.Mvc;
using testeTicketTech.Models;

namespace testeTicketTech.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                // Valida login e senha manualmente
                if (string.IsNullOrEmpty(loginModel.Login) || string.IsNullOrEmpty(loginModel.Senha))
                {
                    ModelState.AddModelError(string.Empty, "Preencha o login e a senha.");
                    return View("Index", loginModel);
                }

                // Valida checkbox manualmente
                if (!loginModel.ConcordaLGPD)
                {
                    ModelState.AddModelError("ConcordaLGPD", "Você deve concordar com a LGPD para continuar.");
                    return View("Index", loginModel);
                }

                // Se tudo estiver ok, redireciona
                return RedirectToAction("Index", "Home");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops! Não conseguimos realizar o seu login. Detalhe: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
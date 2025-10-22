using Microsoft.AspNetCore.Mvc;
using testeTicketTech.Filters;

namespace testeTicketTech.Controllers
{
    [PaginaParaUsuarioLogado]
    public class RestritoController : Controller
    {
        public IActionResult AcessoNegado()
        {
            TempData["MensagemErro"] = "Você não tem permissão para acessar esta funcionalidade.";
            return View();
        }
    }
}

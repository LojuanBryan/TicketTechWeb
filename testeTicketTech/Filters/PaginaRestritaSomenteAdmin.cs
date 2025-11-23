using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using testeTicketTech.Enums;
using testeTicketTech.Models;
using testeTicketTech.Filters;



namespace testeTicketTech.Filters
{
    public class PaginaRestritaSomenteAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Recupera a sessão
            string sessaoUsuario = context.HttpContext.Session.GetString("sessaoUsuarioLogado");

            // Se não estiver logado, redireciona para Login
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    { "controller", "Login" },
                    { "action", "Index" }
                });
                return;
            }

            // Desserializa o usuário
            UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);

            // Se não for admin, redireciona para página de acesso negado
            if (usuario == null || usuario.Perfil != PerfilEnum.Admin)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    { "controller", "Restrito" },
                    { "action", "AcessoNegado" }
                });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}



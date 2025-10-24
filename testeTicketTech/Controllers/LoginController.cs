using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using testeTicketTech.Data;
using testeTicketTech.Helper;
using testeTicketTech.Models;
using testeTicketTech.Repositorios;

namespace testeTicketTech.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ISessao _sessao;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IEmailServico _emailServico;

        public LoginController(ApplicationDbContext db, ISessao sessao, IUsuarioRepositorio usuarioRepositorio, IEmailServico emailServico)
        {
            _db = db;
            _sessao = sessao;
            _usuarioRepositorio = usuarioRepositorio;
            _emailServico = emailServico;
        }

        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoDoUsuario() != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Index", loginModel);

                var senhaCriptografada = Criptografar(loginModel.Senha);

                var usuario = _db.Usuarios.FirstOrDefault(u =>
                    u.Login == loginModel.Login &&
                    u.Senha == senhaCriptografada);


                if (usuario == null)
                {
                    TempData["MensagemErro"] = "Usuário ou senha inválidos.";
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

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Index", "Login");
        }

        // 🔹 Tela para solicitar redefinição de senha
        [HttpGet]
        public IActionResult RedefinirSenha()
        {
            return View();
        }

        // 🔹 Processa solicitação de redefinição
        [HttpPost]
        public IActionResult RedefinirSenha(RedefinirSenhaModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensagemErro"] = "Preencha todos os campos corretamente.";
                return View(model);
            }

            var usuario = _usuarioRepositorio.BuscarPorLoginEEmail(model.Login, model.Email);
            if (usuario == null)
            {
                TempData["MensagemErro"] = "Login ou e-mail não encontrados.";
                return View(model);
            }

            var token = Guid.NewGuid().ToString();
            usuario.TokenRedefinicao = token;
            usuario.TokenExpiraEm = DateTime.Now.AddHours(1);
            _usuarioRepositorio.Atualizar(usuario);

            var link = Url.Action("CadastrarNovaSenha", "Login", new { token = token }, Request.Scheme);
            var mensagem = $"Olá, {usuario.Nome}!<br><br>Para redefinir sua senha, clique no link abaixo:<br><a href='{link}'>Redefinir Senha</a>";

            _emailServico.Enviar(usuario.Email, "Redefinição de Senha - Ticket Tech", mensagem);

            TempData["MensagemSucesso"] = "Se os dados estiverem corretos, você receberá um e-mail com instruções.";
            return RedirectToAction("RedefinirSenha");
        }

        // 🔹 Tela para cadastrar nova senha
        [HttpGet]
        public IActionResult CadastrarNovaSenha(string token)
        {
            var model = new NovaSenhaModel { Token = token };
            return View(model);
        }

        // 🔹 Processa nova senha
        [HttpPost]
        public IActionResult CadastrarNovaSenha(NovaSenhaModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = _usuarioRepositorio.BuscarPorToken(model.Token);

            if (usuario == null || !usuario.TokenExpiraEm.HasValue || usuario.TokenExpiraEm.Value < DateTime.Now)
            {
                TempData["MensagemErro"] = "Token inválido ou expirado.";
                return RedirectToAction("RedefinirSenha");
            }

            usuario.Senha = Criptografar(model.NovaSenha);
            usuario.TokenRedefinicao = null;
            usuario.TokenExpiraEm = null;
            _usuarioRepositorio.Atualizar(usuario);

            TempData["MensagemSucesso"] = "Senha redefinida com sucesso!";
            return RedirectToAction("Index");
        }


        // 🔹 Utilitário de criptografia
        private string Criptografar(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}

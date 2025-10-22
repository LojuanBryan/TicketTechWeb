using Microsoft.AspNetCore.Mvc;
using testeTicketTech.Data;
using testeTicketTech.Enums;
using testeTicketTech.Models;

public class CadastroController : Controller
{
    private readonly ApplicationDbContext _db;

    public CadastroController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(UsuarioModel usuario)
    {
        if (ModelState.IsValid)
        {
            usuario.Perfil = PerfilEnum.Padrao;
            usuario.DataCadastro = DateTime.Now;

            _db.Usuarios.Add(usuario);
            _db.SaveChanges();

            TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";
            return RedirectToAction("Index", "Login");
        }

        return View(usuario);
    }

}


using Microsoft.AspNetCore.Mvc;
using testeTicketTech.Data;

public class UsuariosController : Controller
{
    private readonly ApplicationDbContext _db;

    public UsuariosController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var usuarios = _db.Usuarios.ToList();
        return View(usuarios);
    }
}


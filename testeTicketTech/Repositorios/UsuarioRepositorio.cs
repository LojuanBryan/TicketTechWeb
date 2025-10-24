using testeTicketTech.Data;
using testeTicketTech.Models;

namespace testeTicketTech.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _db;

        public UsuarioRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }

        public UsuarioModel? BuscarPorLoginEEmail(string login, string email)
        {
            return _db.Usuarios.FirstOrDefault(u => u.Login == login && u.Email == email);
        }

        public UsuarioModel? BuscarPorToken(string token)
        {
            return _db.Usuarios.FirstOrDefault(u => u.TokenRedefinicao == token);
        }

        public void Atualizar(UsuarioModel usuario)
        {
            _db.Usuarios.Update(usuario);
            _db.SaveChanges();
        }
    }
}


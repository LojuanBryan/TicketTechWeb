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

        public UsuarioModel? BuscarPorLoginEEmail(string? login, string? email)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(email))
                return null;

            var loginNorm = login.Trim();
            var emailNorm = email.Trim();

            return _db.Usuarios.FirstOrDefault(u => u.Login == loginNorm && u.Email == emailNorm);
        }

        public UsuarioModel? BuscarPorToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            return _db.Usuarios.FirstOrDefault(u => u.TokenRedefinicao == token.Trim());
        }

        public void Atualizar(UsuarioModel usuario)
        {
            _db.Usuarios.Update(usuario);
            _db.SaveChanges();
        }

        public List<UsuarioModel> BuscarTodos()
        {
            return _db.Usuarios.ToList();
        }
    }
}



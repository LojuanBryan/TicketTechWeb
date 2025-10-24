using testeTicketTech.Models;

namespace testeTicketTech.Repositorios
{
    public interface IUsuarioRepositorio
    {
        UsuarioModel BuscarPorLoginEEmail(string login, string email);
        UsuarioModel BuscarPorToken(string token);
        void Atualizar(UsuarioModel usuario);
    }
}




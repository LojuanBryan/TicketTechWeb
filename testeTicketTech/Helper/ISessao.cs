using testeTicketTech.Models;

namespace testeTicketTech.Helper
{
    public interface ISessao
    {
        void CriarSessaoDoUsuario(UsuarioModel usuario);
        void RemoverSessaoDoUsuario ();
        UsuarioModel BuscarSessaoDoUsuario();

    }
}

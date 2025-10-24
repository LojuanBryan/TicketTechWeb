using testeTicketTech.Models;

namespace testeTicketTech.Repositorios
{
    public interface IChamadoRepositorio
    {
        List<Chamados> BuscarTodos();
        List<Chamados> BuscarTodosComUsuarios(int UsuarioId);
        Chamados? BuscarPorId(int id);
        void Adicionar(Chamados chamado);
        void Atualizar(Chamados chamado);
        void Remover(int id);
    }
}


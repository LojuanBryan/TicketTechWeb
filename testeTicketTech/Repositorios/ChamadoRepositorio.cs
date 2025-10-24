using Microsoft.EntityFrameworkCore;
using testeTicketTech.Data;
using testeTicketTech.Models;

namespace testeTicketTech.Repositorios
{
    public class ChamadoRepositorio : IChamadoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public List<Chamados> BuscarTodos()
        {
            return _db.Chamados.Include(c => c.Usuario).ToList();
        }


        public ChamadoRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Chamados> BuscarTodosComUsuarios(int usuarioId)
        {
            return _db.Chamados.Where(x => x.UsuarioId == usuarioId).ToList();
        }


        public Chamados? BuscarPorId(int id)
        {
            return _db.Chamados.Include(c => c.Usuario).FirstOrDefault(c => c.ChamadoId == id);
        }

        public void Adicionar(Chamados chamado)
        {
            _db.Chamados.Add(chamado);
            _db.SaveChanges();
        }

        public void Atualizar(Chamados chamado)
        {
            _db.Chamados.Update(chamado);
            _db.SaveChanges();
        }

        public void Remover(int id)
        {
            var chamado = BuscarPorId(id);
            if (chamado != null)
            {
                _db.Chamados.Remove(chamado);
                _db.SaveChanges();
            }
        }
    }
}



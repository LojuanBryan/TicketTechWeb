using Microsoft.EntityFrameworkCore;
using testeTicketTech.Data.Map;
using testeTicketTech.Models;

namespace testeTicketTech.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<Chamados> Chamados { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChamadoMap());

            base.OnModelCreating(modelBuilder); 
        }
    }
}

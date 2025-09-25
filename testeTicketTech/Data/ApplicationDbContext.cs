using Microsoft.EntityFrameworkCore;
using testeTicketTech.Models;

namespace testeTicketTech.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<Chamados> Chamados { get; set; }     
    }
}

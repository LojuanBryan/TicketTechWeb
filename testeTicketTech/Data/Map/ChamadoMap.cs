using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using testeTicketTech.Models;

namespace testeTicketTech.Data.Map
{
    public class ChamadoMap : IEntityTypeConfiguration<Chamados>
    {
        public void Configure(EntityTypeBuilder<Chamados> builder)
        {
            builder.HasOne(x => x.Usuario)
                   .WithMany(u => u.Chamados)
                   .HasForeignKey(x => x.UsuarioId);

        }
    }
}

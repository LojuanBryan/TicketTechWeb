using System.ComponentModel.DataAnnotations;
//parecequefoi
namespace testeTicketTech.Models
{
    public class Chamados
    {
        [Key]
        public int ChamadoId { get; set; }

        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Preencha o campo Título")]
        [MaxLength(200)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Preencha o campo Dispositivo")]
        [MaxLength(100)]
        public string Dispositivo { get; set; }

        [Required(ErrorMessage = "Preencha o campo Sintomas")]
        [MaxLength(300)]
        public string Sintomas { get; set; }

        [Required(ErrorMessage = "Preencha o Quando ocorreu")]
        [DataType(DataType.Date)]
        public DateTime QuandoOcorreu { get; set; }

        [Required(ErrorMessage = "Preencha Onde ocorreu")]
        [MaxLength(150)]
        public string OndeOcorreu { get; set; }

        [Required(ErrorMessage = "Preencha a Descrição detalhada")]
        [MaxLength(1000)]
        public string DescricaoDetalhada { get; set; }

        public string Status { get; set; } = "Aberto";

        [Required(ErrorMessage = "Preencha a Data do ocorrido")]
        public DateTime DataUltimaAtualizacao { get; set; } = DateTime.Now;
    }
}

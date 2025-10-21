using System.ComponentModel.DataAnnotations;
using testeTicketTech.Enums;

namespace testeTicketTech.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Preencha o e-mail")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Preencha o endereço")]
        public string Endereco { get; set; }

        public PerfilEnum Perfil { get; set; }

        [Required(ErrorMessage = "Preencha a senha")]
        public string Senha { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }
    }
}


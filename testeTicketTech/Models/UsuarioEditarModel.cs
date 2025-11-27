using System.ComponentModel.DataAnnotations;
using testeTicketTech.Enums;

namespace testeTicketTech.Models
{
    public class UsuarioEditarModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Informe o e-mail")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        public string? Endereco { get; set; }

        [Required(ErrorMessage = "Informe o perfil")]
        public PerfilEnum Perfil { get; set; }
    }
}

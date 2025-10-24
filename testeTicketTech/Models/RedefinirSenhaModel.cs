using System.ComponentModel.DataAnnotations;

namespace testeTicketTech.Models
{
    public class RedefinirSenhaModel
    {
        [Required(ErrorMessage = "Informe o login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Informe o e-mail")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }
    }
}


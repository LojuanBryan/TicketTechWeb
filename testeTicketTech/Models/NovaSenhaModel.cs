using System.ComponentModel.DataAnnotations;

namespace testeTicketTech.Models
{
    public class NovaSenhaModel
    {
        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "Informe a nova senha")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string NovaSenha { get; set; }
    }
}




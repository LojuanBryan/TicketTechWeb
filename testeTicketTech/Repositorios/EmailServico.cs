using System.Net;
using System.Net.Mail;

namespace testeTicketTech.Helper
{
    public class EmailServico : IEmailServico
    {
        public void Enviar(string para, string assunto, string corpoHtml)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tickettech.suporte@gmail.com", "ztli vaba xmqk pfde"),
                EnableSsl = true
            };

            var mensagem = new MailMessage("tickettech.suporte@gmail.com", para, assunto, corpoHtml);
            mensagem.IsBodyHtml = true;

            smtp.Send(mensagem);
        }
    }
}




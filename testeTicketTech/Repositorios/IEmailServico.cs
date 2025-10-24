namespace testeTicketTech.Helper
{
    public interface IEmailServico
    {
        void Enviar(string para, string assunto, string corpoHtml);
    }
}




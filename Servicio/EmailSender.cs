namespace RestoApp_Api.Servicio;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks; // Agregar esta directiva

public class EmailSender : ISender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public bool SendEmail(string destinatario, string asunto, string mensajeHtml)
    {
        var from = _config["email"];
        var _password = _config["emailPass"];
        var smtpClient = _config["smptClient"];
        var displayName = "Inmobiliaria T&L";

        try
        {
            MailMessage mail = new MailMessage();
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            mail.From = new MailAddress(from, displayName);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            mail.To.Add(destinatario);

            mail.Subject = asunto;
            mail.Body = mensajeHtml;
            mail.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient(smtpClient, 25))
            {
                client.Credentials = new NetworkCredential(from, _password);
                client.EnableSsl = false;
                client.Send(mail);
            }

            return true;
        }
        catch (SmtpException ex)
        {
            throw new Exception("Error al enviar el email: " + ex.Message);
        }
    }
}

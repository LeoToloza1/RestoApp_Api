namespace RestoApp_Api.Servicio
{
    public interface ISender
    {
        bool SendEmail(string destinatario, string asunto, string mensajeHtml);
    }
}

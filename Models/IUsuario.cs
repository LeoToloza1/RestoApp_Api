namespace RestoApp_Api.Models
{
    public interface IUsuario
    {
        int id { get; }
        string Email { get; }
        string NombreCompleto { get; }
    }
}
namespace RestoApp_Api.Model
{
    public class ClienteDTO
    {
        public int? Id { get; set; }
        public string? Nombre_cliente { get; set; }
        public string? Apellido_cliente { get; set; }
        public string? Direccion_cliente { get; set; }
        public string? Telefono_cliente { get; set; }
        public string? Email_cliente { get; set; }
    }
}
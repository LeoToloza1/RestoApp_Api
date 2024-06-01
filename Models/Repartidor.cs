using System.ComponentModel.DataAnnotations.Schema;

namespace RestoApp_Api.Models
{
    public class Repartidor : IUsuario
    {
        public int id { get; set; }
        public string? nombre_repartidor { get; set; }
        public string? apellido_repartidor { get; set; }
        public string? email_repartidor { get; set; }
        public string? password { get; set; }
        public string? direccion_repartidor { get; set; }
        public string? telefono_repartidor { get; set; }
        public string? avatarUrl { get; set; }
        public bool borrado { get; set; }
        [NotMapped]
        public IFormFile? avatarFile { get; set; }

#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
        public string Email => email_repartidor;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        public string NombreCompleto => $"{nombre_repartidor} {apellido_repartidor}";
    }

}
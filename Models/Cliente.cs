
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using RestoApp_Api.Models;

namespace RestoApp_Api
{
    public class Cliente : IUsuario
    {
        public int id { get; set; }
        [Column("nombre_cliente")]
        public string? Nombre_cliente { get; set; }
        [Column("apellido_cliente")]
        public string? Apellido_cliente { get; set; }
        [Column("email_cliente")]
        public string? Email_cliente { get; set; }
        [Column("password")]
        [JsonIgnore]
        public string? Password { get; set; }
        [Column("direccion_cliente")]
        public string? Direccion_cliente { get; set; }
        [Column("telefono_cliente")]
        public string? Telefono_cliente { get; set; }
        public string? avatarUrl { get; set; }
        public bool borrado { get; set; }
        [NotMapped]
        public IFormFile? AvatarFile { get; set; }
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
        public string Email => Email_cliente;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        public string NombreCompleto => $"{Nombre_cliente} {Apellido_cliente}";
    }
}

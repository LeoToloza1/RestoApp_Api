using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using RestoApp_Api.Models;

namespace RestoApp_Api
{
    public class Restaurante : IUsuario
    {
        [Column("id")]
        [Key]
        public int id { get; set; }

        [Column("nombre_restaurante")]
        public string? Nombre_restaurante { get; set; }

        [Column("direccion_restaurante")]
        public string? Direccion_restaurante { get; set; }

        [Column("email_restaurante")]
        public string? Email_restaurante { get; set; }

        [Column("telefono_restaurante")]
        public string? Telefono_restaurante { get; set; }

        [Column("id_rubro")]
        public int id_rubro { get; set; }

        [Column("password")]
        [JsonIgnore]
        public string? Password { get; set; }

        public string? logo_url { get; set; }
        public bool borrado { get; set; }

        [NotMapped]
        public IFormFile? logoFile { get; set; }

        [ForeignKey("id_rubro")]
        public Rubro? rubro { get; set; }


#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
        public string Email => Email_restaurante;
        public string NombreCompleto => Nombre_restaurante;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
    }
}
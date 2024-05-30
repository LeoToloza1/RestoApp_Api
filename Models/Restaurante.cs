using System.ComponentModel.DataAnnotations.Schema;
using RestoApp_Api.Models;

namespace RestoApp_Api
{
    public class Restaurante
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre_restaurante")]
        public string? Nombre_restaurante { get; set; }
        [Column("direccion_restaurante")]
        public string? Direccion_restaurante { get; set; }
        [Column("email_restaurante")]
        public string? Email_restaurante { get; set; }
        [Column("telefono_restaurante")]
        public string? Telefono_restaurante { get; set; }
        [Column("id_rubro")]
        public int Id_rubro { get; set; }
        [Column("password")]
        public string? Password { get; set; }

        public string? logo_url { get; set; }
        public bool borrado { get; set; }
        [NotMapped]
        public IFormFile? logoFile { get; set; }
        public Rubro? Rubro { get; set; }
    }
}
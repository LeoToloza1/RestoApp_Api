
using System.ComponentModel.DataAnnotations.Schema;

namespace RestoApp_Api
{
    public class Cliente
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre_cliente")]
        public string? Nombre_cliente { get; set; }
        [Column("apellido_cliente")]
        public string? Apellido_cliente { get; set; }
        [Column("email_cliente")]
        public string? Email_cliente { get; set; }
        [Column("password")]
        public string? Password { get; set; }
        [Column("direccion_cliente")]
        public string? Direccion_cliente { get; set; }
        [Column("telefono_cliente")]
        public string? Telefono_cliente { get; set; }
        public string? avatarUrl { get; set; }
        public bool borrado { get; set; }
        [NotMapped]
        public IFormFile? AvatarFile { get; set; }


    }
}

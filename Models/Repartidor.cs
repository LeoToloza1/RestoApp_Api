using System.ComponentModel.DataAnnotations.Schema;

namespace RestoApp_Api.Models
{
    public class Repartidor
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

    }

}
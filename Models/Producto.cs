using System.ComponentModel.DataAnnotations.Schema;

namespace RestoApp_Api.Models
{
    public class Producto
    {
        public int id { get; set; }
        public string? nombre_producto { get; set; }
        public double precio { get; set; }
        public string? descripcion { get; set; }
        public string? imagenUrl { get; set; }
        [NotMapped]
        public IFormFile? imagenFile { get; set; }
        public int restaurante_id { get; set; }
        public Restaurante? restaurante { get; set; }
        public bool borrado { get; set; }


    }

}
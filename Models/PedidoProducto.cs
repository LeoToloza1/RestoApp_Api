using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestoApp_Api.Models
{
    public class PedidoProductos
    {
        [Key]
        public int id { get; set; }
        public int pedido_id { get; set; }
        [ForeignKey("pedido_id")]
        public Pedido? pedido { get; set; }
        public int producto_id { get; set; }
        [ForeignKey("producto_id")]
        public Producto? producto { get; set; }
        public int cantidad { get; set; }
        public bool cancelado { get; set; }

    }
}
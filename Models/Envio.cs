using System.ComponentModel.DataAnnotations.Schema;

namespace RestoApp_Api.Models
{
    public class Envio
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public double costo { get; set; }
        [ForeignKey("id_repartidor")]
        public Repartidor? repartidor { get; set; }
        [ForeignKey("id_pedido")]
        public Pedido? pedido { get; set; }
        public bool estado_envio { get; set; }
    }
}
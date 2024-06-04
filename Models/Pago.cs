using System.ComponentModel.DataAnnotations.Schema;

namespace RestoApp_Api.Models
{
    public class Pago
    {
        public int id { get; set; }
        public bool estado { get; set; }
        public double? monto { get; set; }
        public DateTime fecha_pago { get; set; }
        [ForeignKey("pedido_id")]
        public Pedido? pedido { get; set; }
        public int pedido_id { get; set; }
    }

}
using System.ComponentModel.DataAnnotations.Schema;

namespace RestoApp_Api.Models
{
    public class Pedido
    {
        public int id { get; set; }
        public string? detalle { get; set; }
        public DateTime fecha_pedido { get; set; }
        [ForeignKey("cliente_id")]
        public Cliente? cliente { get; set; }
        public bool cancelado { get; set; }
    }
}
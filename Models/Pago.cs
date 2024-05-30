namespace RestoApp_Api.Models
{
    public class Pago
    {
        public int id { get; set; }
        public string? metodo_pago { get; set; }
        public bool estado { get; set; }
        public double? monto { get; set; }
        public DateTime fecha_pago { get; set; }
        public Pedido? pedido { get; set; }
    }

}
namespace RestoApp_Api.Models
{
    public class PedidoProductos
    {
        public int id { get; set; }
        public Pedido? pedido { get; set; }
        public Producto? producto { get; set; }
        public int cantidad { get; set; }
        public double costo_total { get; set; }
        public bool cancelado { get; set; }

    }
}
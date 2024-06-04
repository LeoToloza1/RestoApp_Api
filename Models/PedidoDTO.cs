
namespace RestoApp_Api.Model
{
    public class PedidoDTO
    {
        public int id { get; set; }
        public string? Detalle { get; set; }
        public ClienteDTO? clienteDTO { get; set; }
        public double total { get; set; }
        public DateTime FechaPedido { get; set; }
        public List<ProductoDto>? Productos { get; set; }
    }
}
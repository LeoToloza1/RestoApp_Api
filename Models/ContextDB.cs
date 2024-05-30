using Microsoft.EntityFrameworkCore;

namespace RestoApp_Api.Models
{
    public class ContextDB : DbContext
    {

        public ContextDB(DbContextOptions<ContextDB> options) : base(options)
        {

        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Restaurante> Restaurantes { get; set; }
        public DbSet<Rubro> Rubros { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Repartidor> Repartidores { get; set; }
        public DbSet<Envio> Envios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<PedidoProductos> PedidosProductos { get; set; }
        public DbSet<Pago> Pagos { get; set; }

    }

}
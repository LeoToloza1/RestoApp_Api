using Microsoft.EntityFrameworkCore;

namespace RestoApp_Api.Models
{
    public class ContextDB : DbContext
    {

        public ContextDB(DbContextOptions<ContextDB> options) : base(options)
        {

        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Restaurante> Restaurante { get; set; }
        public DbSet<Rubro> Rubro { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Repartidor> Repartidor { get; set; }
        public DbSet<Envio> Envio { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<PedidoProductos> Pedido_Producto { get; set; }
        public DbSet<Pago> Pago { get; set; }

    }

}
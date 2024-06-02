using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoApp_Api.Models;
using RestoApp_Api.Repositorios;
using System.Threading.Tasks;

namespace RestoApp_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Cliente")]
    public class PedidoController : ControllerBase
    {
        private readonly RepoPedido _repoPedido;
        private readonly RepoPedidoProducto _repoPedidoProducto;

        public PedidoController(RepoPedido repoPedido, RepoPedidoProducto repoPedidoProducto)
        {
            _repoPedido = repoPedido;
            _repoPedidoProducto = repoPedidoProducto;
        }
        private int GetUsuario()
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var userIdClaim = User.FindFirst("id").Value;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            int userId = int.Parse(userIdClaim);
            return userId;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearPedido([FromForm] Pedido p)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int clienteId = GetUsuario();
            p.cliente_id = clienteId;
            var creado = await _repoPedido.Crear(p);
            if (creado)
            {
                return Ok(p);
            }

            return StatusCode(500, "Error al crear el pedido.");
        }

        [HttpGet("cliente")]
        public async Task<ActionResult<List<Pedido>>> ObtenerPedidosPorCliente()
        {
            int idCliente = GetUsuario();
            var pedidos = await _repoPedido.PedidosPorCliente(idCliente);
            if (pedidos == null || pedidos.Count == 0)
            {
                return NotFound("El cliente aun no realizó pedidos");
            }

            return Ok(pedidos);
        }

        [HttpPatch("cancelar/{id}")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            int idUsuario = GetUsuario();
            var pedido = await _repoPedido.BuscarPorId(id);
            if (pedido == null)
            {
                return NotFound("El pedido no existe.");
            }
            if (pedido.cliente_id != idUsuario)
            {
                return Forbid("No tiene permiso para cancelar este pedido.");
            }
            bool cancelado = await _repoPedido.EliminadoLogico(id);
            if (!cancelado)
            {
                return StatusCode(500, "Hubo un error al intentar cancelar el pedido.");
            }

            return Ok("El pedido se canceló.");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPedido(int id)
        {
            var pedido = await _repoPedido.BuscarPorId(id);
            if (pedido == null)
            {
                return NotFound("Pedido no encontrado");
            }

            var productosPedido = await _repoPedidoProducto.ProdutosPorPedido(id);
            if (productosPedido == null || productosPedido.Count == 0)
            {
                return NotFound("Productos del pedido no encontrados");
            }

            double total = CalcularTotalPedido(productosPedido);
            if (pedido.total != total)
            {
                pedido.total = total;
                var actualizado = await _repoPedido.Actualizar(pedido);
                if (!actualizado)
                {
                    return StatusCode(500, "Error actualizando el total del pedido en la base de datos.");
                }
            }

            var result = ConstruirResultado(pedido, productosPedido);
            return Ok(result);
        }

        private double CalcularTotalPedido(List<PedidoProductos> productosPedido)
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            return productosPedido
                .Where(pp => pp.producto != null)
                .Sum(pp => pp.producto.precio * pp.cantidad);
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
        }

        private object ConstruirResultado(Pedido pedido, List<PedidoProductos> productosPedido)
        {
            return new
            {
                Pedido = new
                {
                    pedido.id,
                    pedido.detalle,
                    pedido.fecha_pedido,
                    Cliente = new
                    {
                        pedido.cliente?.id,
                        pedido.cliente?.Nombre_cliente,
                        pedido.cliente?.Apellido_cliente,
                        pedido.cliente?.Direccion_cliente,
                        pedido.cliente?.Telefono_cliente,
                        pedido.cliente?.avatarUrl,
                    },
                    pedido.cancelado,
                    Total = pedido.total
                },
                Productos = productosPedido.Select(pp => new
                {
                    pp.id,
                    Producto = new
                    {
                        pp.producto?.id,
                        pp.producto?.nombre_producto,
                        pp.producto?.precio,
                        pp.producto?.descripcion,
                        pp.producto?.imagenUrl,
                    },
                    pp.cantidad,
                })
            };
        }

    }
}

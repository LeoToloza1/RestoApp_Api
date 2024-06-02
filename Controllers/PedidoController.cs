using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoApp_Api.Model;
using RestoApp_Api.Models;
using RestoApp_Api.Repositorios;
using System.Collections;
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
        private readonly RepoProducto _repoProducto;

        public PedidoController(RepoPedido repoPedido, RepoPedidoProducto repoPedidoProducto, RepoProducto repoProducto)
        {
            _repoPedido = repoPedido;
            _repoPedidoProducto = repoPedidoProducto;
            _repoProducto = repoProducto;
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
        public async Task<IActionResult> CrearPedido([FromBody] PedidoDTO pedidoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int clienteId = GetUsuario();

            var pedido = new Pedido
            {
                cliente_id = clienteId,
                detalle = pedidoDto.Detalle,
                fecha_pedido = pedidoDto.FechaPedido,
                total = 0 // Se calculará después
            };

            var creado = await _repoPedido.Crear(pedido);
            if (!creado)
            {
                return StatusCode(500, "Error al crear el pedido.");
            }

            double total = 0;
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            foreach (var prodDto in pedidoDto.Productos)
            {
                var producto = await _repoProducto.BuscarPorId(prodDto.ProductoId);
                if (producto == null)
                {
                    return BadRequest($"Producto con ID {prodDto.ProductoId} no encontrado.");
                }

                var pedidoProducto = new PedidoProductos
                {
                    pedido_id = pedido.id,
                    producto_id = prodDto.ProductoId,
                    cantidad = prodDto.Cantidad,
                    cancelado = false
                };

                var productoCreado = await _repoPedidoProducto.Crear(pedidoProducto);
                if (!productoCreado)
                {
                    return StatusCode(500, "Error al crear el producto del pedido.");
                }

                total += producto.precio * prodDto.Cantidad;
            }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

            pedido.total = total;
            var actualizado = await _repoPedido.Actualizar(pedido);
            if (!actualizado)
            {
                return StatusCode(500, "Error al actualizar el total del pedido.");
            }

            return Ok(pedido);
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
            int idCliente = GetUsuario();

            var pedido = await _repoPedido.BuscarPorId(id);
            if (pedido == null)
            {
                return NotFound("Pedido no encontrado");
            }
            if (pedido.cliente_id != idCliente)
            {
                return Forbid("No tiene permiso para acceder a este pedido.");
            }

            var productosPedido = await _repoPedidoProducto.ProdutosPorPedido(id);
            if (productosPedido == null || productosPedido.Count == 0)
            {
                return NotFound("Productos del pedido no encontrados");
            }

            var result = ConstruirResultado(pedido, productosPedido);
            return Ok(result);
        }

        private object ConstruirResultado(Pedido pedido, List<PedidoProductos> productosPedido)
        {
            var pedidoDto = new PedidoDTO
            {
                Detalle = pedido.detalle,
                FechaPedido = pedido.fecha_pedido,
                total = pedido.total,
                clienteDTO = new ClienteDTO
                {
                    Id = pedido.cliente?.id,
                    Nombre_cliente = pedido.cliente?.Nombre_cliente,
                    Apellido_cliente = pedido.cliente?.Apellido_cliente,
                    Direccion_cliente = pedido.cliente?.Direccion_cliente,
                    Telefono_cliente = pedido.cliente?.Telefono_cliente,
                }
            };

#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var productosDto = productosPedido.Select(pp => new ProductoDto
            {
                ProductoId = pp.producto.id,
                Nombre_producto = pp.producto?.nombre_producto,
                Precio = pp.producto.precio,
                Descripcion = pp.producto?.descripcion,
                ImagenUrl = pp.producto?.imagenUrl,
                Cantidad = pp.cantidad
            }).ToList();
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

            return new { Pedido = pedidoDto, Productos = productosDto };
        }

    }
}

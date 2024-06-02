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

        public PedidoController(RepoPedido repo)
        {
            _repoPedido = repo;
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
            var pedidoExistente = await _repoPedido.BuscarPorId(id);
            if (pedidoExistente == null)
            {
                return NotFound("No existe el pedido.");
            }
            return Ok(pedidoExistente);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using RestoApp_Api.Repositorios;
using RestoApp_Api.Models;

namespace RestoApp_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class EnvioController : ControllerBase
    {
        private readonly RepoEnvio _repoEnvio;
        private readonly RepoRepartidor _repoRepartidor;
        private readonly RepoPedido _repoPedido;
        public EnvioController(RepoEnvio repoEnvio, RepoRepartidor repo, RepoPedido repoPedido)
        {
            _repoEnvio = repoEnvio;
            _repoRepartidor = repo;
            _repoPedido = repoPedido;
        }

        [HttpPost("crear/{id}")]
        public async Task<IActionResult> GenerarEnvio(int id)
        {
            try
            {
                var repartidorAleatorio = await _repoRepartidor.RepartidorAleatorio();
                var pedido = await _repoPedido.BuscarPorId(id);

                var nuevoEnvio = new Envio
                {
                    id_pedido = id,
                    costo = pedido.total * 0.20,
                    id_repartidor = repartidorAleatorio.id,
                    fecha = DateTime.Now,
                };

                var creado = await _repoEnvio.Crear(nuevoEnvio);

                if (creado)
                {
                    return Ok(nuevoEnvio);
                }
                else
                {
                    return StatusCode(500, "Error al crear el envío.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al generar el envío: {ex.Message}");
            }
        }



    }
}
using Microsoft.AspNetCore.Mvc;
using RestoApp_Api.Repositorios;
using RestoApp_Api.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestoApp_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly RepoProducto _repoProducto;

        public ProductoController(RepoProducto repoProducto)
        {
            _repoProducto = repoProducto;
        }

        [HttpGet("activos/{restauranteId}")]
        public async Task<ActionResult<List<Producto>>> ObtenerActivosPorRestaurante(int restauranteId)
        {
            var productos = await _repoProducto.ObtenerActivosPorRestaurante(restauranteId);
            return Ok(productos);
        }

        [HttpGet("todos/{restauranteId}")]
        public async Task<ActionResult<List<Producto>>> ObtenerTodosPorRestaurante(int restauranteId)
        {
            var productos = await _repoProducto.ObtenerTodosPorRestaurante(restauranteId);
            return Ok(productos);
        }
    }
}

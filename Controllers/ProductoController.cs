using Microsoft.AspNetCore.Mvc;
using RestoApp_Api.Repositorios;
using RestoApp_Api.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace RestoApp_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly RepoProducto _repoProducto;
        private readonly IWebHostEnvironment environment;

        public ProductoController(RepoProducto repoProducto, IWebHostEnvironment e)
        {
            _repoProducto = repoProducto;
            environment = e;
        }
        private int GetUsuario() //para restaurantes
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var userIdClaim = User.FindFirst("id").Value;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            int userId = int.Parse(userIdClaim);
            return userId;
        }
        [HttpPost]
        [HttpPost]
        [Authorize(Roles = "Restaurante")]
        public async Task<ActionResult> AltaProducto([FromForm] Producto p)
        {
            int idRestaurante = GetUsuario();
            p.restaurante_id = idRestaurante;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (p.imagenFile != null)
            {
                var result = await GuardarAvatar(p.imagenFile);
                if (result.Item1)
                {
                    p.imagenUrl = result.Item2;
                }
                else
                {
                    return BadRequest("El archivo proporcionado no es una imagen válida.");
                }
            }

            bool exito = await _repoProducto.Crear(p);
            if (!exito)
            {
                return StatusCode(500);
            }

            return Ok(p);
        }
        //--------------------------------------------------------------

        [HttpPost("editar")]
        [Authorize(Roles = "Restaurante")]
        public async Task<ActionResult> EditarProducto([FromForm] Producto p)
        {
            int idRestaurante = GetUsuario();
            p.restaurante_id = idRestaurante;
            Producto pExistente = await _repoProducto.BuscarPorId(p.id);
            if (pExistente == null)
            {
                return NotFound("El producto no existe.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (p.imagenFile != null)
            {
                var result = await GuardarAvatar(p.imagenFile);
                if (result.Item1)
                {
                    pExistente.imagenUrl = result.Item2;
                }
                else
                {
                    return BadRequest("El archivo proporcionado no es una imagen válida.");
                }
            }

            pExistente.descripcion = p.descripcion;
            pExistente.nombre_producto = p.nombre_producto;
            pExistente.precio = p.precio;
            pExistente.borrado = p.borrado;

            bool exito = await _repoProducto.Actualizar(pExistente);
            if (!exito)
            {
                return StatusCode(500, "Error al actualizar el producto.");
            }

            return Ok(pExistente);
        }

        [HttpGet("activos/{restauranteId}")]//en android despues sacar este parametro
        public async Task<ActionResult<List<Producto>>> ObtenerActivosPorRestaurante(int restauranteId)
        {
            var productos = await _repoProducto.ObtenerActivosPorRestaurante(restauranteId);
            return Ok(productos);
        }

        [HttpGet("todos")]
        [Authorize(Roles = "Restaurante")]
        public async Task<ActionResult<List<Producto>>> ObtenerTodosPorRestaurante(int restauranteId)
        {
            var productos = await _repoProducto.ObtenerTodosPorRestaurante(restauranteId);
            return Ok(productos);
        }

        [HttpGet("buscar")]
        [AllowAnonymous]
        public async Task<IActionResult> BuscarPorNombre([FromQuery] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest("El nombre de búsqueda no puede estar vacío");
            }

            var productos = await _repoProducto.BuscarPorNombre(nombre);

            if (productos == null || !productos.Any())
            {
                return NotFound(new { message = "No se encontraron restaurantes" });
            }

            return Ok(productos);
        }
        private async Task<(bool, string)> GuardarAvatar(IFormFile imagenFile)
        {
            if (!ImagenValida(imagenFile))
            {
#pragma warning disable CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
                return (false, null);
#pragma warning restore CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
            }

            string uploadsFolder = Path.Combine(environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder); // Crear la carpeta si no existe

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagenFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imagenFile.CopyToAsync(stream);
                }
                return (true, fileName);
            }
            catch (Exception)
            {
#pragma warning disable CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
                return (false, null);
#pragma warning restore CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
            }
        }
        private bool ImagenValida(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            // Validacion de las extensiones 
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            return allowedExtensions.Contains(extension);
        }
    }
}

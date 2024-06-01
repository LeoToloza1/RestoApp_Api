using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoApp_Api.Repositorios;
using RestoApp_Api.Servicio;

namespace RestoApp_Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Restaurante")]
    public class RestauranteController : Controller
    {
        private readonly RepoRestaurante _repo;
        private readonly Auth _auth;
        private readonly IWebHostEnvironment environment;

        public RestauranteController(RepoRestaurante restaurante, Auth auth, IWebHostEnvironment hostEnviroment)
        {
            _repo = restaurante;
            this._auth = auth;
            environment = hostEnviroment;
        }

        private int GetUsuario()
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var userIdClaim = User.FindFirst("id").Value;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            int userId = int.Parse(userIdClaim);
            return userId;
        }

        [HttpGet]
        public async Task<ActionResult> VerPerfil()
        {
            int idUsuario = GetUsuario();
            Restaurante restaurante = await _repo.BuscarPorId(idUsuario);
            restaurante.Password = null;
            return Ok(restaurante);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> login(string email, string password)
        {
            bool acces = await _repo.Login(email, password);
            Restaurante restaurante = await _repo.buscarPorEmail(email);

            if (restaurante == null)
            {
                return NotFound("Por favor intente de nuevo o registrese en la plataforma");
            }

            string jwtToken = _auth.GenerarToken(restaurante);
            if (acces)
            {
                return Ok(jwtToken);
            }
            else
            {
                return NotFound("Credenciales incorrectas, intente de nuevo");
            }
        }

        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<ActionResult> Registrarse([FromForm] Restaurante restaurante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            restaurante.borrado = false;

            if (restaurante.logoFile != null)
            {
                var result = await GuardarAvatar(restaurante.logoFile);
                if (result.Item1)
                {
                    restaurante.logo_url = result.Item2;
                }
                else
                {
                    return BadRequest("El archivo proporcionado no es una imagen válida.");
                }
            }

            try
            {
                bool creado = await _repo.Crear(restaurante);
                if (creado)
                {
                    restaurante.Password = null;
                    return Ok(restaurante);
                }
                else
                {
                    return StatusCode(500, "Error al registrar el restaurante.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar el restaurante: {ex.Message}");
            }
        }
        [HttpPost("editar")]
        public async Task<ActionResult> EditarPerfil([FromForm] Restaurante restaurante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int idRestaurante = GetUsuario();
            Restaurante restauranteExistente = await _repo.BuscarPorId(idRestaurante);
            if (restauranteExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            restauranteExistente.Nombre_restaurante = restaurante.Nombre_restaurante;
            restauranteExistente.Email_restaurante = restaurante.Email_restaurante;
            restauranteExistente.Direccion_restaurante = restaurante.Direccion_restaurante;
            restauranteExistente.Telefono_restaurante = restaurante.Telefono_restaurante;
            restauranteExistente.id_rubro = restaurante.id_rubro;

            if (!string.IsNullOrEmpty(restaurante.Password))
            {
                restauranteExistente.Password = HashPass.HashearPass(restaurante.Password);
            }

            if (restaurante.logoFile != null)
            {
                var result = await GuardarAvatar(restaurante.logoFile);
                if (result.Item1)
                {
                    restauranteExistente.logo_url = result.Item2;
                }
                else
                {
                    return BadRequest("El archivo proporcionado no es una imagen válida.");
                }
            }

            try
            {
                bool actualizado = await _repo.Actualizar(restauranteExistente);
                if (actualizado)
                {
                    restauranteExistente.Password = null; // No devolver la contraseña
                    return Ok(restauranteExistente);
                }
                else
                {
                    return StatusCode(500, "Error al actualizar el perfil del usuario.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el perfil del usuario: {ex.Message}");
            }
        }
        [HttpPost("password")]
        public async Task<ActionResult> cambiarPass([FromForm] string pass)
        {
            int id = GetUsuario();
            bool realizado = await _repo.cambiarPassword(id, pass);
            if (realizado)
            {
                return Ok("La contraseña se actualizo exitosamente");
            }
            else
            {
                return StatusCode(500);
            }
        }
        private async Task<(bool, string)> GuardarAvatar(IFormFile avatarFile)
        {
            if (!ImagenValida(avatarFile))
            {
#pragma warning disable CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
                return (false, null);
#pragma warning restore CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
            }

            string uploadsFolder = Path.Combine(environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder); // Crear la carpeta si no existe

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
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
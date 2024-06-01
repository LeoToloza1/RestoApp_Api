using Microsoft.AspNetCore.Mvc;
using RestoApp_Api.Repositorios;
using RestoApp_Api.Servicio;

namespace RestoApp_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : Controller
    {
        private readonly RepoCliente _repoCliente;
        private readonly Auth _auth;
        private readonly IWebHostEnvironment hostingEnvironment;
        public ClienteController(RepoCliente repositorio, Auth auth, IWebHostEnvironment webHostEnvironment)
        {
            this._repoCliente = repositorio;
            this._auth = auth;
            this.hostingEnvironment = webHostEnvironment;

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
            Cliente cliente = await _repoCliente.BuscarPorId(idUsuario);
            cliente.Password = null;
            return Ok(cliente);
        }

        [HttpPost("login")]
        public async Task<ActionResult> login(string email, string password)
        {
            bool acces = await _repoCliente.Login(email, password);
            Cliente cliente = await _repoCliente.buscarPorEmail(email);

            if (cliente == null)
            {
                return NotFound("Por favor intente de nuevo o registrese en la plataforma");
            }

            string jwtToken = _auth.GenerarToken(cliente);
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
        public async Task<ActionResult> Registrarse([FromForm] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            cliente.borrado = false;
            if (cliente.AvatarFile != null)
            {
                if (!ImagenValida(cliente.AvatarFile))
                {
                    return BadRequest("El archivo proporcionado no es una imagen válida.");
                }
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder); // Crear la carpeta si no existe
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(cliente.AvatarFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await cliente.AvatarFile.CopyToAsync(stream);
                    }
                    cliente.avatarUrl = fileName;
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al guardar el archivo: {ex.Message}");
                }
            }

            try
            {
                bool creado = await _repoCliente.Crear(cliente);
                if (creado)
                {
                    return Ok(cliente);
                }
                else
                {
                    return StatusCode(500, "Error al registrar el usuario.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar el usuario: {ex.Message}");
            }
        }

        [HttpPost("editar")]
        public async Task<ActionResult> EditarPerfil([FromForm] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int idUsuario = GetUsuario();
            Cliente clienteExistente = await _repoCliente.BuscarPorId(idUsuario);
            if (clienteExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            clienteExistente.Nombre_cliente = cliente.Nombre_cliente;
            clienteExistente.Apellido_cliente = cliente.Apellido_cliente;
            clienteExistente.Email_cliente = cliente.Email_cliente;
            clienteExistente.Direccion_cliente = cliente.Direccion_cliente;
            clienteExistente.Telefono_cliente = cliente.Telefono_cliente;
            if (!string.IsNullOrEmpty(cliente.Password))
            {
                clienteExistente.Password = HashPass.HashearPass(cliente.Password);
            }
            clienteExistente.borrado = false;

            if (cliente.AvatarFile != null)
            {
                if (!ImagenValida(cliente.AvatarFile))
                {
                    return BadRequest("El archivo proporcionado no es una imagen válida.");
                }
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder); // Crear la carpeta si no existe
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(cliente.AvatarFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await cliente.AvatarFile.CopyToAsync(stream);
                    }
                    clienteExistente.avatarUrl = fileName;
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al guardar el archivo: {ex.Message}");
                }
            }
            try
            {
                bool actualizado = await _repoCliente.Actualizar(clienteExistente);
                if (actualizado)
                {
                    return Ok(clienteExistente);
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

        private bool ImagenValida(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            // Validacion de las extensiones 
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            return allowedExtensions.Contains(extension);
        }
    }
}
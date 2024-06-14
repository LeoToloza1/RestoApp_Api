using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoApp_Api.Repositorios;
using RestoApp_Api.Servicio;

namespace RestoApp_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Cliente")]
    public class ClienteController : Controller
    {
        private readonly RepoCliente _repoCliente;
        private readonly Auth _auth;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly EmailSender _emailSender;

        public ClienteController(RepoCliente repositorio, Auth auth, IWebHostEnvironment webHostEnvironment, EmailSender emailSender)
        {
            this._repoCliente = repositorio;
            this._auth = auth;
            this.hostingEnvironment = webHostEnvironment;
            _emailSender = emailSender;

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
            return Ok(cliente);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> login([FromForm] string email, [FromForm] string password)
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
        [AllowAnonymous]
        public async Task<ActionResult> Registrarse([FromForm] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            cliente.borrado = false;

            if (cliente.AvatarFile != null)
            {
                var result = await GuardarAvatar(cliente.AvatarFile);
                if (result.Item1)
                {
                    cliente.avatarUrl = result.Item2;
                }
                else
                {
                    return BadRequest("El archivo proporcionado no es una imagen válida.");
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

            clienteExistente.Nombre_cliente = cliente.Nombre_cliente != null ? cliente.Nombre_cliente : clienteExistente.Nombre_cliente;
            clienteExistente.Apellido_cliente = cliente.Apellido_cliente != null ? cliente.Apellido_cliente : clienteExistente.Apellido_cliente;
            clienteExistente.Email_cliente = cliente.Email_cliente != null ? cliente.Email_cliente : clienteExistente.Email_cliente;
            clienteExistente.Direccion_cliente = cliente.Direccion_cliente != null ? cliente.Direccion_cliente : clienteExistente.Direccion_cliente;
            clienteExistente.Telefono_cliente = cliente.Telefono_cliente != null ? cliente.Telefono_cliente : clienteExistente.Telefono_cliente;

            if (cliente.AvatarFile != null)
            {
                var result = await GuardarAvatar(cliente.AvatarFile);
                if (result.Item1)
                {
                    clienteExistente.avatarUrl = result.Item2;
                }
                else
                {
                    return BadRequest("El archivo proporcionado no es una imagen válida.");
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
        [HttpPost("password")]
        public async Task<ActionResult> cambiarPass([FromForm] string pass)
        {
            int id = GetUsuario();
            bool realizado = await _repoCliente.cambiarPassword(id, pass);
            if (realizado)
            {
                return Ok("La contraseña se actualizo exitosamente");
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPost("recuperarPass")]
        [AllowAnonymous]
        public async Task<IActionResult> Recovery([FromForm] string email)
        {
            try
            {
                var cliente = await _repoCliente.ObtenerPorEmail(email);
                var dominio = hostingEnvironment.IsDevelopment() ? HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() : "www.restoapp.com.ar";

                if (cliente != null)
                {
                    string token = GeneratePasswordResetToken();
                    string templatePath = Path.Combine(hostingEnvironment.ContentRootPath, "EmailTemplate.html");
                    string templateContent = System.IO.File.ReadAllText(templatePath);
                    string mensajeHtml = templateContent.Replace("{{Token}}", token).Replace("{{Nombre}}", cliente.Nombre_cliente);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                    bool enviado = _emailSender.SendEmail(cliente.Email_cliente, "Restablecer Contraseña", mensajeHtml);
                    if (enviado)
                    {
                        await _repoCliente.cambiarPassword(cliente.id, token);
                        return Ok("Se ha enviado un correo electrónico con una nueva contraseña, por favor revise su correo.");
                    }
                    else
                    {
                        return BadRequest("Error al enviar el correo electrónico para restablecer la contraseña.");
                    }
                }
                else
                {
                    return NotFound("No se encontró ningún cliente con la dirección de correo electrónico proporcionada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al procesar la solicitud: " + ex.Message);
            }
        }

        private string GeneratePasswordResetToken()
        {
            Random rand = new Random(Environment.TickCount);
            string randomChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            string nuevaClave = "";
            for (int i = 0; i < 8; i++)
            {
                nuevaClave += randomChars[rand.Next(0, randomChars.Length)];
            }
            return nuevaClave;
        }


        private async Task<(bool, string)> GuardarAvatar(IFormFile avatarFile)
        {
            if (!ImagenValida(avatarFile))
            {
#pragma warning disable CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
                return (false, null);
#pragma warning restore CS8619 // La nulabilidad de los tipos de referencia del valor no coincide con el tipo de destino
            }

            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
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
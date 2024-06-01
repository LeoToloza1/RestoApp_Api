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
        private int getUsuario()
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var userIdClaim = User.FindFirst("id").Value;
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
            int userId = int.Parse(userIdClaim);
            return userId;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> listarClientes()
        {
            List<Cliente> lista = await _repoCliente.ObtenerTodos();
            return Ok(lista);
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



    }
}
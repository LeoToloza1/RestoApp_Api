using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RestoApp_Api.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace RestoApp_Api.Servicio
{
    public class Auth
    {
        private readonly IConfiguration _config;

        public Auth(IConfiguration config)
        {
            _config = config;
        }

        public string GenerarToken(IUsuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
#pragma warning restore CS8604 // Posible argumento de referencia nulo

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim("id", usuario.id.ToString()),
                    new Claim("FullName", usuario.NombreCompleto)
                }),
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            if (usuario is Cliente)
            {
                ((ClaimsIdentity)tokenDescriptor.Subject).AddClaim(new Claim(ClaimTypes.Role, "Cliente"));
            }
            else if (usuario is Restaurante)
            {
                ((ClaimsIdentity)tokenDescriptor.Subject).AddClaim(new Claim(ClaimTypes.Role, "Restaurante"));
            }
            else if (usuario is Repartidor)
            {
                ((ClaimsIdentity)tokenDescriptor.Subject).AddClaim(new Claim(ClaimTypes.Role, "Repartidor"));
            }
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

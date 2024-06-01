namespace RestoApp_Api.Servicio;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RestoApp_Api.Models;
using Microsoft.Extensions.Configuration;
using System;
public class Auth
{

    private readonly IConfiguration _config;
    public Auth(IConfiguration config)
    {
        _config = config;
    }
    public string GenerarToken(Cliente cliente)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
#pragma warning disable CS8604 // Posible argumento de referencia nulo
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, cliente.Email_cliente), //guardo el mail
                    new Claim(ClaimTypes.Role, "Propietario"), //guardo el rol
                    new Claim ("id", cliente.Id.ToString()), //guardo el id del logueado
                    new Claim ("FullName" , cliente.Nombre_cliente + " " + cliente.Apellido_cliente) //guardo el nombre completo
            }),
            Expires = DateTime.UtcNow.AddDays(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        //genero el token con las claims
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestoApp_Api.Models;
using RestoApp_Api.Repositorios;
using RestoApp_Api.Servicio;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001", "http://*:5000", "https://*:5001");
// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("Mysql");

// Agregar el DbContext utilizando la cadena de conexión
#pragma warning disable CS8604 // Posible argumento de referencia nulo
builder.Services.AddDbContext<ContextDB>(options => options.UseMySQL(connectionString));
#pragma warning restore CS8604 // Posible argumento de referencia nulo

builder.Services.AddScoped<RepoCliente>();
builder.Services.AddScoped<Auth>();
builder.Services.AddScoped<RepoEnvio>();
builder.Services.AddScoped<RepoPago>();
builder.Services.AddScoped<RepoPedido>();
builder.Services.AddScoped<RepoPedidoProducto>();
builder.Services.AddScoped<RepoProducto>();
builder.Services.AddScoped<RepoRepartidor>();
builder.Services.AddScoped<RepoRestaurante>();
builder.Services.AddScoped<RepoRubro>();


var jwtSettings = builder.Configuration.GetSection("Jwt");
var SecretKey = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication()
   .AddJwtBearer(options =>
   {
       options.RequireHttpsMetadata = false;
#pragma warning disable CS8604 // Posible argumento de referencia nulo
       var key = Encoding.ASCII.GetBytes(SecretKey);
#pragma warning restore CS8604 // Posible argumento de referencia nulo

       options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
       {
           ValidateIssuer = false,
           ValidateAudience = false, //este en falso para las pruebas
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = issuer,
           ValidAudience = audience,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
       };
   });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();

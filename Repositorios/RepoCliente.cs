using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestoApp_Api.Repositorios
{
    public class RepoCliente : IRepositorio<Cliente>
    {
        private readonly ContextDB _context;

        public RepoCliente(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> Actualizar(Cliente entity)
        {
            _context.Set<Cliente>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Cliente> BuscarPorId(int id)
        {
            var cliente = await _context.Set<Cliente>().FindAsync(id);
            if (cliente == null)
            {

            }
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return cliente;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo

        }

        public async Task<bool> Crear(Cliente entity)
        {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            entity.Password = HashPass.HashearPass(entity.Password);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            await _context.Cliente.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminadoLogico(int id)
        {
            var entity = await _context.Set<Cliente>().FindAsync(id);
            if (entity == null)
                return false;

            entity.borrado = true;
            _context.Set<Cliente>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Cliente>> ObtenerActivos()
        {
            return await _context.Set<Cliente>().Where(c => !c.borrado).ToListAsync();
        }

        public async Task<List<Cliente>> ObtenerTodos()
        {
            return await _context.Set<Cliente>().ToListAsync();
        }
        public async Task<bool> Login(string email, string password)
        {
            var cliente = await _context.Cliente.SingleOrDefaultAsync(c => c.Email_cliente == email);
            if (cliente == null)
            {
                return false;
            }
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            bool acces = HashPass.VerificarPassword(password, cliente.Password);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            if (acces)
            {
                return true;
            }

            return false;
        }
        public async Task<Cliente> buscarPorEmail(string email)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.Cliente.FirstOrDefaultAsync(c => c.Email_cliente == email);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
        public async Task<bool> cambiarPassword(int id, string passwordNueva)
        {
            var cliente = await BuscarPorId(id);
            if (cliente == null)
            {
                Console.WriteLine("No se encontró el cliente.");
                return false;
            }
            string hashedPassword = HashPass.HashearPass(passwordNueva);
            cliente.Password = hashedPassword;
            _context.SaveChanges();
            return true;
        }
        public async Task<Cliente> ObtenerPorEmail(string email)
        {
            try
            {
                var cliente = await _context.Cliente.SingleAsync(c => c.Email_cliente == email);
                return cliente;
            }
            catch (InvalidOperationException)
            {
                throw new Exception("No se encontró ningún propietario registrado con el correo electrónico especificado.");
            }
        }

    }
}

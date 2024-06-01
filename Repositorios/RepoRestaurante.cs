using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestoApp_Api.Repositorios
{
    public class RepoRestaurante : IRepositorio<Restaurante>
    {
        private readonly ContextDB _context;

        public RepoRestaurante(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> Actualizar(Restaurante entity)
        {
            _context.Set<Restaurante>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Restaurante> BuscarPorId(int id)
        {
            var restaurante = await _context.Set<Restaurante>().FindAsync(id);
            if (restaurante == null)
            {

            }
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return restaurante;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo

        }
        public async Task<bool> Crear(Restaurante entity)
        {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            entity.Password = HashPass.HashearPass(entity.Password);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            await _context.Restaurante.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> EliminadoLogico(int id)
        {
            var entity = await _context.Set<Restaurante>().FindAsync(id);
            if (entity == null)
                return false;
            entity.borrado = true;
            _context.Set<Restaurante>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<Restaurante>> ObtenerActivos()
        {
            return await _context.Set<Restaurante>().Where(r => !r.borrado).ToListAsync();
        }
        public async Task<List<Restaurante>> ObtenerTodos()
        {
            return await _context.Set<Restaurante>().ToListAsync();
        }
        public async Task<bool> Login(string email, string password)
        {
            var restaurante = await _context.Restaurante.SingleOrDefaultAsync(c => c.Email_restaurante == email);
            if (restaurante == null)
            {
                return false;
            }
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            bool acces = HashPass.VerificarPassword(password, restaurante.Password);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            if (acces)
            {
                return true;
            }

            return false;
        }
        public async Task<Restaurante> buscarPorEmail(string email)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.Restaurante.FirstOrDefaultAsync(c => c.Email_restaurante == email);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
        public async Task<bool> cambiarPassword(int id, string passwordNueva)
        {
            var restaurante = await BuscarPorId(id);
            if (restaurante == null)
            {
                Console.WriteLine("No se encontró el restaurante.");
                return false;
            }
            string hashedPassword = HashPass.HashearPass(passwordNueva);
            restaurante.Password = hashedPassword;
            _context.SaveChanges();
            return true;
        }

    }
}
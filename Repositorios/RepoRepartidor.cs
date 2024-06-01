using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestoApp_Api.Repositorios
{
    public class RepoRepartidor : IRepositorio<Repartidor>
    {
        private readonly ContextDB _context;

        public RepoRepartidor(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> Actualizar(Repartidor entity)
        {
            _context.Set<Repartidor>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Repartidor> BuscarPorId(int id)
        {
            var Repartidor = await _context.Set<Repartidor>().FindAsync(id);
            if (Repartidor == null)
            {

            }
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return Repartidor;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo

        }
        public async Task<bool> Crear(Repartidor entity)
        {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            entity.password = HashPass.HashearPass(entity.password);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            await _context.Repartidor.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> EliminadoLogico(int id)
        {
            var entity = await _context.Set<Repartidor>().FindAsync(id);
            if (entity == null)
                return false;
            entity.borrado = true;
            _context.Set<Repartidor>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<Repartidor>> ObtenerActivos()
        {
            return await _context.Set<Repartidor>().Where(r => !r.borrado).ToListAsync();
        }
        public async Task<List<Repartidor>> ObtenerTodos()
        {
            return await _context.Set<Repartidor>().ToListAsync();
        }
        public async Task<bool> Login(string email, string password)
        {
            var Repartidor = await _context.Repartidor.SingleOrDefaultAsync(c => c.email_repartidor == email);
            if (Repartidor == null)
            {
                return false;
            }
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            bool acces = HashPass.VerificarPassword(password, Repartidor.password);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            if (acces)
            {
                return true;
            }

            return false;
        }
        public async Task<Repartidor> buscarPorEmail(string email)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.Repartidor.FirstOrDefaultAsync(c => c.email_repartidor == email);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
        public async Task<bool> cambiarPassword(int id, string passwordNueva)
        {
            var Repartidor = await BuscarPorId(id);
            if (Repartidor == null)
            {
                Console.WriteLine("No se encontró el Repartidor.");
                return false;
            }
            string hashedPassword = HashPass.HashearPass(passwordNueva);
            Repartidor.password = hashedPassword;
            _context.SaveChanges();
            return true;
        }

    }
}
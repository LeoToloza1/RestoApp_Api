using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestoApp_Api.Repositorios
{
    public class RepoEnvio : IRepositorio<Envio>
    {
        private readonly ContextDB _context;

        public RepoEnvio(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> Actualizar(Envio entity)
        {
            try
            {
                _context.Envio.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Envio> BuscarPorId(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.Envio
                .Include(e => e.repartidor)
                .Include(e => e.pedido)
                .FirstOrDefaultAsync(e => e.id == id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public async Task<bool> Crear(Envio entity)
        {
            try
            {
                await _context.Envio.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> EliminadoLogico(int id)
        {
            var entity = await _context.Envio.FindAsync(id);
            if (entity == null)
                return false;

            entity.estado_envio = true;

            try
            {
                _context.Envio.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<List<Envio>> ObtenerActivos()
        {
            return await _context.Envio
                .Include(e => e.repartidor)
                .Include(e => e.pedido)
                .Where(envio => !envio.estado_envio)
                .ToListAsync();
        }

        public async Task<List<Envio>> ObtenerTodos()
        {
            return await _context.Envio
                .Include(e => e.repartidor)
                .Include(e => e.pedido)
                .ToListAsync();
        }
    }
}

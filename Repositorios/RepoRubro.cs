using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestoApp_Api.Repositorios
{
    public class RepoRubro : IRepositorio<Rubro>
    {
        private readonly ContextDB _context;

        public RepoRubro(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> Actualizar(Rubro entity)
        {
            _context.Set<Rubro>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Rubro> BuscarPorId(int id)
        {
            var rubro = await _context.Rubro
              .FirstOrDefaultAsync(r => r.id == id);
            if (rubro == null)
            {
                //devolver una exception
            }
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return rubro;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo

        }
        public async Task<bool> Crear(Rubro entity)
        {
            await _context.Rubro.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> EliminadoLogico(int id)
        {
            var entity = await _context.Set<Rubro>().FindAsync(id);
            if (entity == null)
                return false;
            entity.borrado = true;
            _context.Set<Rubro>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<Rubro>> ObtenerActivos()
        {
            return await _context.Set<Rubro>().Where(r => !r.borrado).ToListAsync();
        }
        public async Task<List<Rubro>> ObtenerTodos()
        {
            return await _context.Set<Rubro>().ToListAsync();
        }

    }
}
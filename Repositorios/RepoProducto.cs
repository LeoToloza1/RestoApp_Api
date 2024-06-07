using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace RestoApp_Api.Repositorios
{
    public class RepoProducto : IRepositorio<Producto>
    {
        private readonly ContextDB _context;

        public RepoProducto(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> Actualizar(Producto entity)
        {
            _context.Set<Producto>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Producto> BuscarPorId(int id)
        {
            var entity = await _context.Set<Producto>().FindAsync(id);
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return entity; // Devuelve null si no encuentra el producto
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public async Task<bool> Crear(Producto entity)
        {
            await _context.Producto.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminadoLogico(int id)
        {
            var entity = await _context.Set<Producto>().FindAsync(id);
            if (entity == null)
                return false;

            entity.borrado = true;
            _context.Set<Producto>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        //metodos para un administrador del sistema
        public async Task<List<Producto>> ObtenerActivos()
        {
            return await _context.Set<Producto>().Where(p => !p.borrado).ToListAsync();
        }

        public async Task<List<Producto>> ObtenerTodos()
        {
            return await _context.Set<Producto>().ToListAsync();
        }
        //metodos para el restuarante
        public async Task<List<Producto>> ObtenerActivosPorRestaurante(int restauranteId)
        {
            return await _context.Set<Producto>()
                .Where(p => !p.borrado && p.restaurante_id == restauranteId)
                    .Include(p => p.restaurante)
                .ToListAsync();
        }

        public async Task<List<Producto>> ObtenerTodosPorRestaurante(int restauranteId)
        {
            return await _context.Set<Producto>()
                .Where(p => p.restaurante_id == restauranteId)

                .ToListAsync();
        }
        public async Task<List<Producto>> BuscarPorNombre(string nombre)
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            return await _context.Producto
            .Where(p => p.nombre_producto.Contains(nombre))
            .ToListAsync();
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
        }



    }
}

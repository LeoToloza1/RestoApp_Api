using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestoApp_Api.Repositorios
{
    public class RepoPedidoProducto : IRepositorio<PedidoProductos>
    {
        private readonly ContextDB _context;

        public RepoPedidoProducto(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> Actualizar(PedidoProductos entity)
        {
            try
            {
                _context.pedido_producto.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<PedidoProductos> BuscarPorId(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.pedido_producto
                .Include(pp => pp.pedido)
                .Include(pp => pp.producto)
                .FirstOrDefaultAsync(pp => pp.id == id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public async Task<bool> Crear(PedidoProductos entity)
        {
            try
            {
                await _context.pedido_producto.AddAsync(entity);
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
            var entity = await _context.pedido_producto.FindAsync(id);
            if (entity == null)
                return false;

            try
            {
                _context.pedido_producto.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<List<PedidoProductos>> ObtenerActivos()
        {
            return await _context.pedido_producto
                .Include(pp => pp.pedido)
                .Include(pp => pp.producto)
                .ToListAsync();
        }

        public async Task<List<PedidoProductos>> ObtenerTodos()
        {
            return await _context.pedido_producto
                .Include(pp => pp.pedido)
                .Include(pp => pp.producto)
                .ToListAsync();
        }

        public async Task<List<PedidoProductos>> ProdutosPorPedido(int id)
        {
            return await _context.pedido_producto
                .Include(pp => pp.pedido)
                .Include(pp => pp.producto)
                .Where(pp => pp.pedido_id == id)
                .ToListAsync();
        }



    }
}

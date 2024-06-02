using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestoApp_Api.Repositorios
{
    public class RepoPedido : IRepositorio<Pedido>
    {
        private readonly ContextDB _context;

        public RepoPedido(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> Actualizar(Pedido entity)
        {
            try
            {
                _context.Pedido.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Pedido> BuscarPorId(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.Pedido
                .Include(p => p.cliente) // Asumiendo que tienes una propiedad de navegación llamada "Cliente"
                .FirstOrDefaultAsync(p => p.id == id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
        public async Task<bool> Crear(Pedido entity)
        {
            try
            {
                await _context.Pedido.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
        //cancelar pedido
        public async Task<bool> EliminadoLogico(int id)
        {
            var entity = await _context.Pedido.FindAsync(id);
            if (entity == null)
                return false;

            entity.cancelado = true;

            try
            {
                _context.Pedido.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<List<Pedido>> ObtenerActivos()
        {
            return await _context.Pedido
                .Include(p => p.cliente)
                .Where(pedido => !pedido.cancelado)
                .ToListAsync();
        }

        public async Task<List<Pedido>> ObtenerTodos()
        {
            return await _context.Pedido
                .Include(p => p.cliente)
                .ToListAsync();
        }

        public async Task<List<Pedido>> PedidosPorCliente(int id)
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            return await _context.Pedido
            .Include(p => p.cliente)
            .Where(p => p.cliente.id == id)
            .ToListAsync();
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
        }


    }
}

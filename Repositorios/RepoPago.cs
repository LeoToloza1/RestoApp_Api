using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestoApp_Api.Repositorios
{
    public class RepoPago : IRepositorio<Pago>
    {
        private readonly ContextDB _context;

        public RepoPago(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> Actualizar(Pago entity)
        {
            try
            {
                _context.Pago.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Pago> BuscarPorId(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.Pago
                .Include(e => e.pedido)
                .FirstOrDefaultAsync(e => e.id == id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public async Task<bool> Crear(Pago entity)
        {
            try
            {
                await _context.Pago.AddAsync(entity);
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
            var entity = await _context.Pago.FindAsync(id);
            if (entity == null)
                return false;
            entity.estado = true;
            try
            {
                _context.Pago.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<List<Pago>> ObtenerActivos()
        {
            return await _context.Pago
                .Include(e => e.pedido)
                .Where(Pago => !Pago.estado)
                .ToListAsync();
        }

        public async Task<List<Pago>> ObtenerTodos()
        {
            return await _context.Pago
                .Include(e => e.pedido)
                .ToListAsync();
        }

        public async Task<bool> PagarPedido(int id)
        {
            //uso de transaccion
            using (var transaccion = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var pago = await BuscarPorId(id);
                    if (!pago.estado)
                    {
                        pago.estado = true;
                        await _context.SaveChangesAsync();
                        await transaccion.CommitAsync();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    await transaccion.RollbackAsync();
                    return false;
                }
            }
        }


    }
}
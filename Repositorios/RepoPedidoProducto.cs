using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
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

        public Task<bool> Actualizar(PedidoProductos entity)
        {
            throw new NotImplementedException();
        }

        public Task<PedidoProductos> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Crear(PedidoProductos entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminadoLogico(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PedidoProductos>> ObtenerActivos()
        {
            throw new NotImplementedException();
        }

        public Task<List<PedidoProductos>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
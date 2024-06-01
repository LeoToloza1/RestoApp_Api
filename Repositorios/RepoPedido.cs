using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
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

        public Task<bool> Actualizar(Pedido entity)
        {
            throw new NotImplementedException();
        }

        public Task<Pedido> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Crear(Pedido entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminadoLogico(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pedido>> ObtenerActivos()
        {
            throw new NotImplementedException();
        }

        public Task<List<Pedido>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
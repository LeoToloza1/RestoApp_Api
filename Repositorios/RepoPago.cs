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

        public Task<bool> Actualizar(Pago entity)
        {
            throw new NotImplementedException();
        }

        public Task<Pago> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Crear(Pago entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminadoLogico(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pago>> ObtenerActivos()
        {
            throw new NotImplementedException();
        }

        public Task<List<Pago>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
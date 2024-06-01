using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestoApp_Api.Repositorios
{
    public class RepoProducto : IRepositorio<Producto>
    {
        private readonly ContextDB _context;

        public RepoProducto(ContextDB context)
        {
            _context = context;
        }

        public Task<bool> Actualizar(Producto entity)
        {
            throw new NotImplementedException();
        }

        public Task<Producto> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Crear(Producto entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminadoLogico(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Producto>> ObtenerActivos()
        {
            throw new NotImplementedException();
        }

        public Task<List<Producto>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
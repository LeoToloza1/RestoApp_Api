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

        public Task<bool> Actualizar(Rubro entity)
        {
            throw new NotImplementedException();
        }

        public Task<Rubro> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Crear(Rubro entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminadoLogico(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Rubro>> ObtenerActivos()
        {
            throw new NotImplementedException();
        }

        public Task<List<Rubro>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
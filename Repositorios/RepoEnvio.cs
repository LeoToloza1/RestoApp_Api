using Microsoft.EntityFrameworkCore;
using RestoApp_Api.Models;
using RestoApp_Api.Servicio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestoApp_Api.Repositorios
{
    public class RepoEnvio : IRepositorio<Envio>
    {
        private readonly ContextDB _context;

        public RepoEnvio(ContextDB context)
        {
            _context = context;
        }

        public Task<bool> Actualizar(Envio entity)
        {
            throw new NotImplementedException();
        }

        public Task<Envio> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Crear(Envio entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminadoLogico(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Envio>> ObtenerActivos()
        {
            throw new NotImplementedException();
        }

        public Task<List<Envio>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
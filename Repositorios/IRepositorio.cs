namespace RestoApp_Api.Repositorios
{
    public interface IRepositorio<T>
    {
        Task<List<T>> ObtenerTodos();
        Task<List<T>> ObtenerActivos();
        Task<bool> Crear(T entity);
        Task<bool> Actualizar(T entity);
        Task<bool> EliminadoLogico(int id);
        Task<T> BuscarPorId(int id);
    }
}


namespace RestoApp_Api.Repositorios
{
    public interface IRepositorio<T>
    {
        List<T> ObtenerTodos();
        List<T> ObtenerActivos();
        bool Crear(T entity);
        bool Actualizar(T entity);
        bool EliminadoLogico(int id);
        T BuscarPorId(int id);
    }
}
namespace GestionInventario;

public interface IProductoRepositorio
{
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task ActualizarAsync(Producto producto);
}

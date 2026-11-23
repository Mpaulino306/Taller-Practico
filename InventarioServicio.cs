namespace GestionInventario;

public class InventarioServicio
{
    private readonly IProductoRepositorio _repositorio;

    public InventarioServicio(IProductoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<bool> DescontarStockAsync(int productoId, int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que cero.", nameof(cantidad));

        var producto = await _repositorio.ObtenerPorIdAsync(productoId);
        if (producto is null)
            return false;

        if (producto.Stock < cantidad)
            return false;

        producto.Stock -= cantidad;
        await _repositorio.ActualizarAsync(producto);
        return true;
    }

    public async Task<bool> TieneStockBajoAsync(int productoId)
    {
        var producto = await _repositorio.ObtenerPorIdAsync(productoId);
        if (producto is null)
            return false;

        return producto.Stock <= producto.StockMinimo;
    }
}

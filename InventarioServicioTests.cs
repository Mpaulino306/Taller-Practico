using GestionInventario;
using Moq;
using Xunit;

namespace GestionInventario.Tests;

public class InventarioServicioTests
{
    private readonly Mock<IProductoRepositorio> _repositorio = new();
    private InventarioServicio CrearServicio() => new(_repositorio.Object);

    [Fact]
    public async Task DescontarStock_CantidadValida_ReduceStockYActualizaRepositorio()
    {
        // Arrange: el mock simula el repositorio externo.
        var producto = new Producto { Id = 1, Nombre = "Teclado", Stock = 10, StockMinimo = 2 };
        _repositorio.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);
        var servicio = CrearServicio();

        // Act
        var resultado = await servicio.DescontarStockAsync(1, 3);

        // Assert
        Assert.True(resultado);
        Assert.Equal(7, producto.Stock);
        _repositorio.Verify(r => r.ActualizarAsync(producto), Times.Once);
    }

    [Fact]
    public async Task DescontarStock_ProductoNoExiste_RetornaFalso()
    {
        _repositorio.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((Producto?)null);
        var servicio = CrearServicio();

        var resultado = await servicio.DescontarStockAsync(99, 1);

        Assert.False(resultado);
        _repositorio.Verify(r => r.ActualizarAsync(It.IsAny<Producto>()), Times.Never);
    }

    [Fact]
    public async Task DescontarStock_CantidadMayorAlStock_NoActualiza()
    {
        var producto = new Producto { Id = 2, Nombre = "Mouse", Stock = 2, StockMinimo = 1 };
        _repositorio.Setup(r => r.ObtenerPorIdAsync(2)).ReturnsAsync(producto);
        var servicio = CrearServicio();

        var resultado = await servicio.DescontarStockAsync(2, 5);

        Assert.False(resultado);
        Assert.Equal(2, producto.Stock);
        _repositorio.Verify(r => r.ActualizarAsync(It.IsAny<Producto>()), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DescontarStock_CantidadInvalida_LanzaExcepcion(int cantidad)
    {
        var servicio = CrearServicio();

        await Assert.ThrowsAsync<ArgumentException>(
            () => servicio.DescontarStockAsync(1, cantidad));
    }

    [Fact]
    public async Task TieneStockBajo_StockIgualAlMinimo_RetornaVerdadero()
    {
        var producto = new Producto { Id = 3, Nombre = "Monitor", Stock = 5, StockMinimo = 5 };
        _repositorio.Setup(r => r.ObtenerPorIdAsync(3)).ReturnsAsync(producto);
        var servicio = CrearServicio();

        var resultado = await servicio.TieneStockBajoAsync(3);

        Assert.True(resultado);
    }

    [Fact]
    public async Task TieneStockBajo_StockMayorAlMinimo_RetornaFalso()
    {
        var producto = new Producto { Id = 4, Nombre = "Cable", Stock = 8, StockMinimo = 3 };
        _repositorio.Setup(r => r.ObtenerPorIdAsync(4)).ReturnsAsync(producto);
        var servicio = CrearServicio();

        var resultado = await servicio.TieneStockBajoAsync(4);

        Assert.False(resultado);
    }
}

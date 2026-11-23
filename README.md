# Proyecto TDD: Gestión de Inventario

Proyecto académico en C# que demuestra pruebas unitarias con xUnit y dobles de prueba con Moq.

## Requisitos
- .NET 8 SDK
- Acceso a Internet la primera vez para restaurar paquetes NuGet

## Ejecutar las pruebas
Desde la carpeta raíz:

```bash
dotnet restore tests/GestionInventario.Tests/GestionInventario.Tests.csproj
dotnet test tests/GestionInventario.Tests/GestionInventario.Tests.csproj
```

## Qué se prueba
- Descuento de stock correcto y verificación de la llamada al repositorio (mock).
- Producto inexistente.
- Stock insuficiente.
- Cantidades inválidas (teoría con dos casos).
- Detección de stock mínimo y stock por encima del mínimo.

## Uso de Moq
`Setup(...).ReturnsAsync(...)` prepara el comportamiento simulado del repositorio.
`Verify(..., Times.Once)` y `Verify(..., Times.Never)` comprueban si se llamó o no al método de actualización.

## Nota sobre TDD
Para documentar el ciclo Red-Green-Refactor, ejecuta primero una prueba antes de implementar la lógica y guarda capturas reales de cada etapa. El código final contiene las pruebas automatizadas; no se incluyen capturas simuladas.

using Modelos;


namespace Business.Services.IServices;

public interface ICatalogoProductoService
{
    public Task<IEnumerable<CatalogoProducto>> GetCatalogoProductosAync(object data, CancellationToken cancellationToken = default);
}
using Modelos;

namespace Business.Services.IServices;

public interface IProductosListosService
{
    public Task<IEnumerable<Produccion>> GetProductosListosAync(object data, CancellationToken cancellationToken = default);
}
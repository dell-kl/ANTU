using Modelos;

namespace Business.Services.IServices;

public interface IFabricacionService
{
    public Task<IEnumerable<Produccion>> GetProduccionAync(object data, CancellationToken cancellationToken = default);
}
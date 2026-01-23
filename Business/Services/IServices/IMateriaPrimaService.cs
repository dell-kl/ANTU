using System.Collections.ObjectModel;
using Modelos;

namespace Business.Services.IServices;

public interface IMateriaPrimaService
{
    public Task<IEnumerable<MateriaPrimaProducto>> GetMateriaPrimaAync(object data, CancellationToken cancellationToken = default);
}
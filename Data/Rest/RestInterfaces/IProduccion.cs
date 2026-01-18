using Modelos;
using Modelos.RequestDto;

namespace Data.Rest.RestInterfaces;

public interface IProduccion : IRestGeneric<ProduccionReqestDto, Produccion>
{
    public Task<bool> cambiarEstadoProduccionAFabricado(IEnumerable<Produccion> productosListos, Func<Task>? ejecutarTarea = null);
}
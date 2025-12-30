using ANTU.Models;
using ANTU.Models.RequestDto;

namespace ANTU.Resources.Rest.RestInterfaces;

public interface IProduccion : IRestGeneric<ProduccionReqestDto, Produccion>
{
    public Task<bool> cambiarEstadoProduccionAFabricado(IEnumerable<Produccion> productosListos, Func<Task>? ejecutarTarea = null);
}
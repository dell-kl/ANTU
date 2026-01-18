using Modelos;
using Modelos.RequestDto;

namespace Data.Rest.RestInterfaces;

public interface IProduccionLista : IRestGeneric<FabricadoRequestDto, DataCatalogoProduccion>
{
    public Task<IEnumerable<DataCatalogoProduccion>> ObtenerDatosCatalogoProduccion(string identificadorCatalogoProduccion, object data);
        
    public Task<IEnumerable<ProductosListos>> ObtenerInformacionProductosBodega(string identificadorProduccion, object data);

    public Task<bool> ActualizarEstadoAFinalizado(string identificadorProduccion, Func<Task> ejecutarTarea);
}
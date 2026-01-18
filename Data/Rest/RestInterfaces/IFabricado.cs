using Modelos;
using Modelos.RequestDto;

namespace Data.Rest.RestInterfaces;

public interface IFabricado : IRestGeneric<FabricadoRequestDto, Produccion>
{
    
}
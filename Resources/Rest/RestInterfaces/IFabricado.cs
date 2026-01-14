using ANTU.Models;
using ANTU.Models.RequestDto;

namespace ANTU.Resources.Rest.RestInterfaces;

public interface IFabricado : IRestGeneric<FabricadoRequestDto, Produccion>
{
    
}
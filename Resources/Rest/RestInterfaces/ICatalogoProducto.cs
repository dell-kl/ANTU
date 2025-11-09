using ANTU.Models;
using ANTU.Models.RequestDto;
using System.Collections.ObjectModel;
using ANTU.Models.Dto;

namespace ANTU.Resources.Rest.RestInterfaces
{
    public interface ICatalogoProducto : IRestGeneric<CatalogoProductoRequestDto, CatalogoProducto>
    {
        Task<Dictionary<string, object>> SaveImages(ObservableCollection<FileResultExtensible> fileResultExtensible, string guid, bool activarVentanasAlerta = false, Func<Task>? ejecutarTask = null);

        Task<IEnumerable<DataCatalogProducto>> GetDataCatalogProducto(object data, string GuidCatalogProduct);
    }
}

using Modelos;
using Modelos.RequestDto;
using System.Collections.ObjectModel;
using Modelos.Dto;

namespace Data.Rest.RestInterfaces
{
    public interface ICatalogoProducto : IRestGeneric<CatalogoProductoRequestDto, CatalogoProducto>
    {
        Task<Dictionary<string, object>> SaveImages(ObservableCollection<FileResultExtensible> fileResultExtensible, string guid, bool activarVentanasAlerta = false, Func<Task>? ejecutarTask = null);

        Task<IEnumerable<DataCatalogProducto>> GetDataCatalogProducto(object data, string GuidCatalogProduct);

        Task<bool> AddDatosVentaDataCatalogProduct(CatalogoProductoRequestDto catalogProductRequestDto, Func<Task> ejecutarTarea);

        Task<CatalogoProductoDetalle> GetDataCatalogProductoDetalle(string GuidCatalogProduct);

        Task<bool> DeleteImages(ICollection<DataImage> dataImages, Func<Task>? ejecutarTask = null);
    }
}

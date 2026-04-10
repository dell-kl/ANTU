using Modelos;
using Modelos.RequestDto;
using System.Collections.ObjectModel;
using Modelos.Dto;
using Modelos.ResultDto;

namespace Data.Rest.RestInterfaces
{
    public interface ICatalogoProducto : IRestGeneric<CatalogoProductoRequestDto, CatalogoProducto>
    {
        Task<RequestResultDto<object>> RegistrarImagenesCatalogoProducto(ObservableCollection<FileResultExtensible> fileResultExtensibles, string guid);
        
        Task<IEnumerable<DataCatalogProducto>> GetDataCatalogProducto(object data, string GuidCatalogProduct);

        Task<bool> AddDatosVentaDataCatalogProduct(CatalogoProductoRequestDto catalogProductRequestDto, Func<Task> ejecutarTarea);

        Task<CatalogoProductoDetalle> GetDataCatalogProductoDetalle(string GuidCatalogProduct);

        Task<bool> DeleteImages(ICollection<DataImage> dataImages, Func<Task>? ejecutarTask = null);
    }
}

using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using System.Collections.ObjectModel;

namespace Data.Rest.RestInterfaces
{
    public interface IMateriaPrima : IRestGeneric<MateriaPrimaRequestDto, MateriaPrimaProducto>
    {
        Task<Dictionary<string, object>> SaveImages(ObservableCollection<FileResultExtensible> fileResultExtensible, string guid, bool activarVentanasAlerta = false, Func<Task>? ejecutarTask = null);

        Task<MateriaPrimaDetalle> MateriaPrimaDetalles(string guid);

        Task<bool> AgregarStockMateriaPrima(StockMateriaPrimaRequestDto stockMateriaPrima, Func<Task>? ejecutarTask = null);

        Task<bool> EditarDatosMateriaPrima(MateriaPrimaRequestDto materiaPrimaRequestDTO, Func<Task>? ejecutarTask = null);

        Task<bool> DeleteImages(ICollection<DataImage> dataImages, Func<Task>? ejecutarTask = null);

        Task<IEnumerable<KgSeguimiento>> GetKgSeguimientos(object data, string guidMateriaPrima);
    }
}

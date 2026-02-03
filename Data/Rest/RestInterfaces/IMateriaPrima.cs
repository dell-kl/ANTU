using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using System.Collections.ObjectModel;
using Modelos.ResultDto;

namespace Data.Rest.RestInterfaces
{
    public interface IMateriaPrima : IRestGeneric<MateriaPrimaRequestDto, MateriaPrimaProducto>
    {
        Task<Dictionary<string, object>> SaveImages(ObservableCollection<FileResultExtensible> fileResultExtensible, string guid, bool activarVentanasAlerta = false, Func<Task>? ejecutarTask = null);

        Task<RequestResultDto<string>> RegistrarImagenesMateriaPrima(ObservableCollection<FileResultExtensible> fileResultExtensibles, string guid);
        
        Task<RequestResultDto<MateriaPrimaDetalle>> MateriaPrimaDetalles(string guid);

        Task<bool> AgregarStockMateriaPrima(StockMateriaPrimaRequestDto stockMateriaPrima, Func<Task>? ejecutarTask = null);

        Task<bool> EditarDatosMateriaPrima(MateriaPrimaRequestDto materiaPrimaRequestDTO, Func<Task>? ejecutarTask = null);

        Task<bool> DeleteImages(ICollection<DataImage> dataImages, Func<Task>? ejecutarTask = null);

        Task<RequestResultDto<IEnumerable<KgSeguimiento>>> GetKgSeguimientos(MateriaPrimaDetalle datos);
    }
}

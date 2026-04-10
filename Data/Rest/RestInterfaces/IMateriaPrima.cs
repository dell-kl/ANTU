using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using System.Collections.ObjectModel;
using Modelos.ResultDto;

namespace Data.Rest.RestInterfaces
{
    public interface IMateriaPrima : IRestGeneric<MateriaPrimaRequestDto, MateriaPrimaProducto>
    {
        Task<RequestResultDto<object>> RegistrarImagenesMateriaPrima(ObservableCollection<FileResultExtensible> fileResultExtensibles, string guid);
        
        Task<RequestResultDto<MateriaPrimaDetalle>> MateriaPrimaDetalles(string guid);

        Task<RequestResultDto<KgSeguimiento>> AgregarStockMateriaPrima(StockMateriaPrimaRequestDto stockMateriaPrima);

        Task<RequestResultDto<bool>> EditarDatosMateriaPrima(MateriaPrimaRequestDto materiaPrimaRequestDTO);

        Task<RequestResultDto<bool>> DeleteImages(ICollection<DataImage> dataImages);

        Task<RequestResultDto<IEnumerable<KgSeguimiento>>> GetKgSeguimientos(MateriaPrimaDetalle datos);
    }
}

using System.Collections.ObjectModel;
using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using Modelos.ResultDto;

namespace Business.Services.IServices;

public interface IMateriaPrimaService
{
    public Task<RequestResultDto<IEnumerable<MateriaPrimaProducto>>> GetMateriaPrimaAync(object data);
    
    public Task<RequestResultDto<object>> RegistrarMateriaPrima(MateriaPrimaFormulario materiaPrimaFormulario, ObservableCollection<FileResultExtensible> listadoImagenes);

    public Task<RequestResultDto<(MateriaPrimaDetalle, IEnumerable<KgSeguimiento>)>> ObtenerDatosMateriaPrimaDetalle(string identificador);
    
    public Task<RequestResultDto<object>> RegistarImagenesMateriaPrima(ObservableCollection<FileResultExtensible> listadoImagenes, string identificador);

    public Task<RequestResultDto<bool>> EliminarImagenesMateriaPrima(ICollection<DataImage> dataImages);
    
    public Task<RequestResultDto<IEnumerable<KgSeguimiento>>> GetKgSeguimientoMateriaPrimaDetalle(MateriaPrimaDetalle materiaPrimaDetalle);

    public Task<RequestResultDto<KgSeguimiento>> AgregarStockMateriaPrima(MateriaPrimaFormulario materiaPrimaFormulario, string identificador);

    public Task<RequestResultDto<bool>> EditarDatosMateriaPrima(MateriaPrimaRequestDto materiaPrimaRequestDto);
}
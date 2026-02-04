using System.Collections.ObjectModel;
using Modelos;
using Modelos.Dto;
using Modelos.ResultDto;

namespace Business.Services.IServices;

public interface IMateriaPrimaService
{
    public Task<RequestResultDto<IEnumerable<MateriaPrimaProducto>>> GetMateriaPrimaAync(object data);

    public Task<RequestResultDto<string>> RegistrarMateriaPrima(MateriaPrimaFormulario materiaPrimaFormulario, ObservableCollection<FileResultExtensible> listadoImagenes);

    public Task<RequestResultDto<(MateriaPrimaDetalle, IEnumerable<KgSeguimiento>)>> ObtenerDatosMateriaPrimaDetalle(string identificador);
    
    public Task RegistarImagenesMateriaPrima(ObservableCollection<FileResultExtensible> listadoImagenes);

    public Task<RequestResultDto<IEnumerable<KgSeguimiento>>> GetKgSeguimientoMateriaPrimaDetalle(MateriaPrimaDetalle materiaPrimaDetalle);
}
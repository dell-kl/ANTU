using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using Modelos.ResultDto;

namespace Business.Services;

[SupportedOSPlatform("Android")]
public class MateriaPrimaService : IMateriaPrimaService
{
    private bool _hasMore = true;
    private readonly IRestManagement _restManagement;
    
    
    public MateriaPrimaService(IRestManagement restManagement)
    {
        this._restManagement = restManagement;
    }
    
    //Este codigo realizaremos algunas modificaciones mas
    //adelante con implementacion de cache,
    //por el momento tendremos esta
    //configuracion.
    public async Task<RequestResultDto<IEnumerable<MateriaPrimaProducto>>> GetMateriaPrimaAync(object data)
    {
        if (!_hasMore)
            return new List<MateriaPrimaProducto>();
        
        RequestResultDto<IEnumerable<MateriaPrimaProducto>> resultado = await _restManagement.MateriaPrima.Get(data);
        
        if ( resultado.Value != null && resultado!.Value.Count() < 10)
            _hasMore = false;

        return resultado!;
    }

    public async Task<RequestResultDto<string>> RegistrarMateriaPrima(MateriaPrimaFormulario materiaPrimaFormulario, ObservableCollection<FileResultExtensible> listadoImagenes)
    {
        string identificador = Guid.NewGuid().ToString();
        
        RequestResultDto<string> resultado = await _restManagement.MateriaPrima.Add(
            new MateriaPrimaRequestDto()
            {
                id_dto = identificador,
                nombre_dto = materiaPrimaFormulario.MateriaPrima,
                KgMonitoringDtos = new List<KgSeguimientoRequestDto>()
                {
                    new KgSeguimientoRequestDto()
                    {
                        id_dto = null,
                        cantidad_dto = materiaPrimaFormulario.Cantidad,
                        kg_standard = materiaPrimaFormulario.KgStandard,
                        price_dto = (decimal) materiaPrimaFormulario.Precio
                    }   
                }
            });
        return  await resultado.Bind(_restManagement.MateriaPrima.RegistrarImagenesMateriaPrima, listadoImagenes, identificador);
    }

    public async Task<RequestResultDto<(MateriaPrimaDetalle, IEnumerable<KgSeguimiento>)>> ObtenerDatosMateriaPrimaDetalle(string identificador)
    {
        RequestResultDto<MateriaPrimaDetalle> resultado = await _restManagement.MateriaPrima.MateriaPrimaDetalles(identificador);

        if (resultado.Value != null)
        {
            resultado.Value.Identificador = identificador;
            resultado.Value.NValoresListadoKgSeguimiento = 0;
        }
        
        RequestResultDto<(MateriaPrimaDetalle, IEnumerable<KgSeguimiento>)> resultadoNuevo = await resultado
            .Combine(_restManagement.MateriaPrima.GetKgSeguimientos);
        
        return resultadoNuevo;
    }


    public Task RegistarImagenesMateriaPrima(ObservableCollection<FileResultExtensible> listadoImagenes)
    {
        throw new NotImplementedException();
    }

    public async Task<RequestResultDto<IEnumerable<KgSeguimiento>>> GetKgSeguimientoMateriaPrimaDetalle(MateriaPrimaDetalle materiaPrimaDetalle)
    {
        RequestResultDto<IEnumerable<KgSeguimiento>> resultado = await _restManagement.MateriaPrima.GetKgSeguimientos(materiaPrimaDetalle);

        return resultado;
    }
}
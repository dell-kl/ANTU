using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using Modelos.ResultDto;
using Syncfusion.Maui.DataSource.Extensions;

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
    
    public async Task<IEnumerable<MateriaPrimaProducto>> GetMateriaPrimaAync(object data, CancellationToken cancellationToken = default)
    {
        if (!_hasMore)
            return new List<MateriaPrimaProducto>();
        
        var resultado = await _restManagement.MateriaPrima.Get(data);

        resultado = resultado.ToObservableCollection();
        if (resultado!.Count() < 10)
            _hasMore = false;

        return resultado!;
    }

    public async Task<RequestResultDto<string>> RegistrarMateriaPrima(MateriaPrimaFormulario materiaPrimaFormulario, ObservableCollection<FileResultExtensible> listadoImagenes)
    {
        
        // RequestResultDto<string> resultado = await _restManagement.MateriaPrima.Add(
        //     new MateriaPrimaRequestDto()
        //     {
        //         id_dto = Guid.NewGuid().ToString(),
        //         nombre_dto = materiaPrimaFormulario.MateriaPrima,
        //         KgMonitoringDtos = new List<KgSeguimientoRequestDto>()
        //         {
        //             new KgSeguimientoRequestDto()
        //             {
        //                 id_dto = null,
        //                 cantidad_dto = materiaPrimaFormulario.Cantidad,
        //                 kg_standard = materiaPrimaFormulario.KgStandard,
        //                 price_dto = (decimal) materiaPrimaFormulario.Precio
        //             }   
        //         }
        //     });
        
        
        // return resultado;
        return null;
    }

    public async Task RegistarImagenesMateriaPrima(ObservableCollection<FileResultExtensible> listadoImagenes)
    {
        
    }
}
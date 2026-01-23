using System.Runtime.Versioning;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using Modelos;
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

}
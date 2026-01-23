using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using Modelos;
using Syncfusion.Maui.DataSource.Extensions;

namespace Business.Services;

public class CatalogoProductoService : ICatalogoProductoService
{
    private readonly IRestManagement _restManagement;
    private bool _hasMore = true;
    
    public CatalogoProductoService(IRestManagement restManagement)
    {
        this._restManagement = restManagement;
        
    }
    
    public async Task<IEnumerable<CatalogoProducto>> GetCatalogoProductosAync(object data, CancellationToken cancellationToken = default)
    {
        if (!_hasMore)
            return new List<CatalogoProducto>();
        
        var resultado = await _restManagement.CatalogoProduct.Get(data);

        resultado = resultado.ToObservableCollection();
        if (resultado!.Count() < 10)
            _hasMore = false;

        return resultado!;
    }
}
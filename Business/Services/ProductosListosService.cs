using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using Modelos;
using Syncfusion.Maui.DataSource.Extensions;

namespace Business.Services;

public class ProductosListosService : IProductosListosService
{
    private readonly IRestManagement _restManagement;
    private bool _hasMore = true;
    
    public ProductosListosService(IRestManagement restManagement)
    {
        this._restManagement = restManagement;
    }
    
    public async Task<IEnumerable<Produccion>> GetProductosListosAync(object data, CancellationToken cancellationToken = default)
    {
        if (!_hasMore)
            return new List<Produccion>();
        
        var resultado = await _restManagement.Fabricado.Get(data);

        resultado = resultado.ToObservableCollection();
        if (resultado!.Count() < 10)
            _hasMore = false;

        return resultado!;
    }
}
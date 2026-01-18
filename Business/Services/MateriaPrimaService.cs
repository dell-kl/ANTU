using System.Collections.ObjectModel;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using Modelos;

namespace Business.Services;

public class MateriaPrimaService : IMateriaPrimaService
{
    private int _pageSize = 10;
    private int _maxItems = 100;
    private bool _isLoading = false;
    
    private readonly IRestManagement _restManagement;
    
    public MateriaPrimaService(IRestManagement restManagement)
    {
        this._restManagement = restManagement;
    }


    public async Task GetMateriaPrimaAync(object data, ObservableCollection<MateriaPrimaProducto> listadoMateriaPrimaProductos)
    {
        var tokenSource = new CancellationTokenSource();
        CancellationToken token = tokenSource.Token;
        
        var resultado = await _restManagement.MateriaPrima.Get(data);
        
        foreach (var r in resultado)
        {
            listadoMateriaPrimaProductos.Add(r);
        }
    }

}
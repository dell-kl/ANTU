using System.Collections.ObjectModel;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using Modelos.ResultDto;

namespace Business.Services;

public class CatalogoProductoService : ICatalogoProductoService
{
    private readonly IRestManagement _restManagement;
    private bool _hasMore = true;
    
    public CatalogoProductoService(IRestManagement restManagement)
    {
        this._restManagement = restManagement;
    }
    
    public async Task<RequestResultDto<IEnumerable<CatalogoProducto>>> GetCatalogoProductosAync(object data, CancellationToken cancellationToken = default)
    {
        if (!_hasMore)
            return new List<CatalogoProducto>();
        //
        RequestResultDto<IEnumerable<CatalogoProducto>> resultado = await _restManagement.CatalogoProduct.Get(data);
        
        if ( resultado.Success && resultado!.Value.Count() < 10 )
            _hasMore = false;
        
        return resultado;
    }
    
    
    public async Task<RequestResultDto<object>> RegistrarCatalogoProductoAsync(CatalogoProductoFormulario catalogoProductoFormulario, ObservableCollection<FileResultExtensible> listadoImagenes)
    {
        string identificador = Guid.NewGuid().ToString();

        RequestResultDto<object> resultado = await _restManagement.CatalogoProduct.Add(
            new Modelos.RequestDto.CatalogoProductoRequestDto()
            {
                identificador = identificador,
                nombreProducto = catalogoProductoFormulario.NombreProducto!,
                dataCatalogProducts = new List<DataProduct>()
                {
                    new DataProduct()
                    {
                        precio = (decimal) catalogoProductoFormulario.DatosVentas.Precio,
                        pesoKg = catalogoProductoFormulario.DatosVentas.Kg,
                        cantidadTotal = catalogoProductoFormulario.DatosVentas.Cantidad
                    }
                }
            });
        if (!listadoImagenes.Any())
            return resultado;
        
        return  await resultado.Bind(_restManagement.CatalogoProduct.RegistrarImagenesCatalogoProducto, listadoImagenes, identificador);
    }
}
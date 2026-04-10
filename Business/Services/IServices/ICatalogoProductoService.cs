using System.Collections.ObjectModel;
using Modelos;
using Modelos.ResultDto;
using Modelos.Dto;


namespace Business.Services.IServices;

public interface ICatalogoProductoService
{
    public Task<RequestResultDto<IEnumerable<CatalogoProducto>>> GetCatalogoProductosAync(object data, CancellationToken cancellationToken = default);
    public Task<RequestResultDto<object>> RegistrarCatalogoProductoAsync(CatalogoProductoFormulario catalogoProductoFormulario, ObservableCollection<FileResultExtensible> listadoImagenes);
}   
using System.Collections.ObjectModel;
using Modelos;

namespace Business.Services.IServices;

public interface IMateriaPrimaService
{
    public Task GetMateriaPrimaAync(object data, ObservableCollection<MateriaPrimaProducto> listadoMateriaPrimaProductos);
}
using ANTU.Models;
using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using System.Collections.ObjectModel;

namespace ANTU.Resources.Rest.RestInterfaces
{
    public interface IMateriaPrima : IRestGeneric<MateriaPrimaRequestDto, MateriaPrimaProducto>
    {
         Task<bool> SaveImages(ObservableCollection<FileResultExtensible> fileResultExtensible, string guid);
    }
}

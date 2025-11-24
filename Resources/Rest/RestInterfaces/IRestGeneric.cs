using ANTU.Models.Dto;
using System.Collections.ObjectModel;

namespace ANTU.Resources.Rest.RestInterfaces
{
    public interface IRestGeneric<TRequestPost, TRequestGet> where TRequestPost: class
                                                            where TRequestGet : class
    {
        public Task<bool> Add(TRequestPost data, Func<Task> ejecutarTarea, bool mostrarMensajes = false);
        public Task<bool> Add(TRequestPost data, Func<Task> ejecutarTarea, ObservableCollection<FileResultExtensible> fileResultExtensibles);
        public Task<IEnumerable<TRequestGet>> Get(object data);
        public Task<bool> Update(TRequestPost data, Func<Task> ejecutarTarea);
        public void Delete();
    }
}

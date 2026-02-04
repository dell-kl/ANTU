using Modelos.Dto;
using System.Collections.ObjectModel;
using Modelos.ResultDto;

namespace Data.Rest.RestInterfaces
{
    public interface IRestGeneric<TRequestPost, TRequestGet> where TRequestPost: class
                                                            where TRequestGet : class
    {
        public Task<RequestResultDto<string>> Add(TRequestPost data);
        public Task<RequestResultDto<string>> Add(TRequestPost data, ObservableCollection<FileResultExtensible> fileResultExtensibles);
        public Task<RequestResultDto<IEnumerable<TRequestGet>>> Get(object data);
        public Task<bool> Update(TRequestPost data, Func<Task> ejecutarTarea);
        public void Delete();
    }
}

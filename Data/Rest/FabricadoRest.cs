using System.Collections.ObjectModel;
using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using Data.Rest.RestInterfaces;
using Modelos.ResultDto;
using Newtonsoft.Json;

namespace Data.Rest;

public class FabricadoRest : IFabricado
{
    internal readonly HttpClient httpClient;

    public FabricadoRest(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public Task<RequestResultDto<string>> Add(FabricadoRequestDto data)
    {
        throw new NotImplementedException();
    }

    public Task<RequestResultDto<string>> Add(FabricadoRequestDto data, ObservableCollection<FileResultExtensible> fileResultExtensibles)
    {
        throw new NotImplementedException();
    }

    public async Task<RequestResultDto<IEnumerable<Produccion>>> Get(object data)
    {
        IEnumerable<Produccion> listProduccion = new List<Produccion>();

        using HttpResponseMessage httpResponse =  await httpClient.GetAsync($"{Endpoints.ENDPOINTS_FABRICADO[0]}/{data}");
            
        if (httpResponse.IsSuccessStatusCode)
            listProduccion = JsonConvert.DeserializeObject<IEnumerable<Produccion>>(await httpResponse.Content.ReadAsStringAsync())!;

        return null;
    }

    public Task<bool> Update(FabricadoRequestDto data, Func<Task> ejecutarTarea)
    {
        throw new NotImplementedException();
    }

    public void Delete()
    {
        throw new NotImplementedException();
    }
}
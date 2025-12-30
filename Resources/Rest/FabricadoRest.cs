using System.Collections.ObjectModel;
using ANTU.Models;
using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Rest.RestInterfaces;
using Newtonsoft.Json;

namespace ANTU.Resources.Rest;

public class FabricadoRest : IFabricado
{
    internal readonly HttpClient httpClient;
    
    public FabricadoRest(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public Task<bool> Add(FabricadoRequestDto data, Func<Task> ejecutarTarea, bool mostrarMensajes = false)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Add(FabricadoRequestDto data, Func<Task> ejecutarTarea, ObservableCollection<FileResultExtensible> fileResultExtensibles)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Produccion>> Get(object data)
    {
        IEnumerable<Produccion> listProduccion = new List<Produccion>();

        using HttpResponseMessage httpResponse =  await httpClient.GetAsync($"{Endpoints.ENDPOINTS_FABRICADO[0]}/{data}");
            
        if (httpResponse.IsSuccessStatusCode)
            listProduccion = JsonConvert.DeserializeObject<IEnumerable<Produccion>>(await httpResponse.Content.ReadAsStringAsync())!;

        return listProduccion;
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
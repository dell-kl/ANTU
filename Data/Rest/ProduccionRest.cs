using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mime;
using System.Text;
using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using Data.Rest.RestInterfaces;
using Modelos.ResultDto;
using Newtonsoft.Json;

namespace Data.Rest;

public class ProduccionRest : IProduccion
{
    internal readonly HttpClient httpClient;
    
    public ProduccionRest(HttpClient httpClient)
    {
        this.httpClient = httpClient; 
    }
    
    public async Task<RequestResultDto<object>> Add(ProduccionReqestDto data)
    {
        try
        {
            using StringContent json = new(
                JsonConvert.SerializeObject(data), 
                Encoding.UTF8, 
                MediaTypeNames.Application.Json);

            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS_FABRICACION[1], json);
        
            if(httpResponse.StatusCode != HttpStatusCode.OK)
                return Result.Failure(await httpResponse.Content.ReadAsStringAsync(), httpResponse.StatusCode);
        
            return Result.Success(await httpResponse.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            return Result.Failure<string>(Error.Exception(e).ToList());
        }
    }

    public Task<RequestResultDto<string>> Add(ProduccionReqestDto data, ObservableCollection<FileResultExtensible> fileResultExtensibles)
    {
        throw new NotImplementedException();
    }

    public async Task<RequestResultDto<IEnumerable<Produccion>>> Get(object data)
    {
        IEnumerable<Produccion> listProduccion = new List<Produccion>();

        using HttpResponseMessage httpResponse =  await httpClient.GetAsync($"{Endpoints.ENDPOINTS_FABRICACION[0]}/{data}");
            
        if (httpResponse.IsSuccessStatusCode)
            listProduccion = JsonConvert.DeserializeObject<IEnumerable<Produccion>>(await httpResponse.Content.ReadAsStringAsync())!;

        return null;
    }

    public Task<bool> Update(ProduccionReqestDto data, Func<Task> ejecutarTarea)
    {
        throw new NotImplementedException();
    }

    public void Delete()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> cambiarEstadoProduccionAFabricado(IEnumerable<Produccion> productosListos, Func<Task>? ejecutarTarea)
    {
        using StringContent stringContent = new(
            JsonConvert.SerializeObject(productosListos),
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );
        
        using HttpResponseMessage httpResponse = await httpClient.PutAsync(Endpoints.ENDPOINTS_FABRICACION[2], stringContent);

        if (ejecutarTarea != null)
            await ejecutarTarea();
        
        // if (httpResponse.IsSuccessStatusCode)
        //     await _mensaje.MensajeCorrecto("Estado Produccion", await httpResponse.Content.ReadAsStringAsync());
        // else
        //     await _mensaje.MensajeError("Estado Produccion", await httpResponse.Content.ReadAsStringAsync());

        return (httpResponse.IsSuccessStatusCode) ? true : false;
    }
}
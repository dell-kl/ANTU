using System.Collections.Immutable;
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

public class ProduccionListaRest : IProduccionLista
{
    private HttpClient httpClient;
    
    public ProduccionListaRest(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async Task<RequestResultDto<string>> Add(FabricadoRequestDto data)
    {
        try
        {
            using StringContent json = new(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                MediaTypeNames.Application.Json
            );
        
            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS_FABRICADO[2], json);
        
            if(httpResponse.StatusCode != HttpStatusCode.OK)
                return Result.Failure(await httpResponse.Content.ReadAsStringAsync(), httpResponse.StatusCode);

            return Result.Success(await httpResponse.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            return Result.Failure<string>(Error.Exception(e).ToList());
        }
    }

    public Task<RequestResultDto<string>> Add(FabricadoRequestDto data, ObservableCollection<FileResultExtensible> fileResultExtensibles)
    {
        throw new NotImplementedException();
    }

    public Task<RequestResultDto<IEnumerable<DataCatalogoProduccion>>> Get(object data)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(FabricadoRequestDto data, Func<Task> ejecutarTarea)
    {
        throw new NotImplementedException();
    }

    public void Delete()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DataCatalogoProduccion>> ObtenerDatosCatalogoProduccion(string identificadorCatalogoProduccion, object data)
    {
        IEnumerable<DataCatalogoProduccion> listadoDataCatalogoProduccion = new List<DataCatalogoProduccion>();

        using HttpResponseMessage httpResponse =  await httpClient.GetAsync($"{Endpoints.ENDPOINTS_FABRICADO[1]}/{identificadorCatalogoProduccion}/{data}");
            
        if (httpResponse.IsSuccessStatusCode)
            listadoDataCatalogoProduccion = JsonConvert.DeserializeObject<IEnumerable<DataCatalogoProduccion>>(await httpResponse.Content.ReadAsStringAsync())!;
        
        return listadoDataCatalogoProduccion;
    }

    public async Task<IEnumerable<ProductosListos>> ObtenerInformacionProductosBodega(string identificadorProduccion, object data)
    {
        IEnumerable<ProductosListos> listadoProductosListos = new List<ProductosListos>();
        
        using HttpResponseMessage httpResponse =  await httpClient.GetAsync($"{Endpoints.ENDPOINTS_FABRICADO[3]}/{identificadorProduccion}/{data}");

        if (httpResponse.IsSuccessStatusCode)
            listadoProductosListos = JsonConvert.DeserializeObject<IEnumerable<ProductosListos>>(await httpResponse.Content.ReadAsStringAsync())!;

        return listadoProductosListos; 
    }

    public async Task<bool> ActualizarEstadoAFinalizado(string identificadorProduccion, Func<Task> ejecutarTarea)
    {
        using HttpResponseMessage httpResponse = await httpClient.PostAsync($"{Endpoints.ENDPOINTS_FABRICADO[4]}/{identificadorProduccion}", null);

        await ejecutarTarea();
        
        // if (httpResponse.IsSuccessStatusCode)
        //     await _mensaje.MensajeCorrecto("Proceso Completado", await httpResponse.Content.ReadAsStringAsync());
        // else if (httpResponse.StatusCode == HttpStatusCode.InternalServerError)
        //     await _mensaje.MensajeError("Error Proceso", await httpResponse.Content.ReadAsStringAsync());
        
        return (httpResponse.IsSuccessStatusCode) ? true : false;
    }
}
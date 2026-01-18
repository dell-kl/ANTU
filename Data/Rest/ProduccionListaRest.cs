using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mime;
using System.Text;
using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using Data.Rest.RestInterfaces;
using Newtonsoft.Json;

namespace Data.Rest;

public class ProduccionListaRest : IProduccionLista
{
    private HttpClient httpClient;
    
    public ProduccionListaRest(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async Task<bool> Add(FabricadoRequestDto data, Func<Task> ejecutarTarea, bool mostrarMensajes = false)
    {
        using StringContent json = new(
            JsonConvert.SerializeObject(data),
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );
        
        using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS_FABRICADO[2], json);

        await ejecutarTarea();  

        // if (httpResponse.IsSuccessStatusCode && mostrarMensajes) 
        //     await _mensaje.MensajeCorrecto("Guardado Exitosamente", await httpResponse.Content.ReadAsStringAsync());
        // else if (httpResponse.StatusCode == HttpStatusCode.InternalServerError && mostrarMensajes)
        //     await _mensaje.MensajeError("Error Guardado", await httpResponse.Content.ReadAsStringAsync());

        return (httpResponse.IsSuccessStatusCode) ? true : false;
    }

    public Task<bool> Add(FabricadoRequestDto data, Func<Task> ejecutarTarea, ObservableCollection<FileResultExtensible> fileResultExtensibles)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DataCatalogoProduccion>> Get(object data, Func<Task>? ejecutarTarea = null)
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
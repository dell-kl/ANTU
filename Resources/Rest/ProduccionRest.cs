using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mime;
using System.Text;
using ANTU.Models;
using ANTU.Resources.Utilidades;
using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Rest.RestInterfaces;
using Newtonsoft.Json;

namespace ANTU.Resources.Rest;

public class ProduccionRest : IProduccion
{
    internal readonly HttpClient httpClient;
    
    public ProduccionRest(HttpClient httpClient)
    {
        this.httpClient = httpClient; 
    }
    
    public async Task<bool> Add(ProduccionReqestDto data, Func<Task> ejecutarTarea, bool mostrarMensajes = false)
    {
        using StringContent json = new(
            JsonConvert.SerializeObject(data), 
            Encoding.UTF8, 
            MediaTypeNames.Application.Json);

        using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS_FABRICACION[1], json);

        await ejecutarTarea();

        if (httpResponse.IsSuccessStatusCode && mostrarMensajes) 
            await Mensaje.MensajeCorrecto("Guardado Exitosamente", await httpResponse.Content.ReadAsStringAsync());
        else if (httpResponse.StatusCode == HttpStatusCode.InternalServerError && mostrarMensajes)
            await Mensaje.MensajeError("Error Guardado", await httpResponse.Content.ReadAsStringAsync());

        return (httpResponse.IsSuccessStatusCode) ? true : false;
    }

    public Task<bool> Add(ProduccionReqestDto data, Func<Task> ejecutarTarea, ObservableCollection<FileResultExtensible> fileResultExtensibles)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Produccion>> Get(object data)
    {
        IEnumerable<Produccion> listProduccion = new List<Produccion>();

        using HttpResponseMessage httpResponse =  await httpClient.GetAsync($"{Endpoints.ENDPOINTS_FABRICACION[0]}/{data}");
            
        if (httpResponse.IsSuccessStatusCode)
            listProduccion = JsonConvert.DeserializeObject<IEnumerable<Produccion>>(await httpResponse.Content.ReadAsStringAsync())!;

        return listProduccion;
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
        
        if (httpResponse.IsSuccessStatusCode)
            await Mensaje.MensajeCorrecto("Estado Produccion", await httpResponse.Content.ReadAsStringAsync());
        else
            await Mensaje.MensajeError("Estado Produccion", await httpResponse.Content.ReadAsStringAsync());

        return (httpResponse.IsSuccessStatusCode) ? true : false;
    }
}
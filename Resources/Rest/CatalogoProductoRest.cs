using ANTU.Models;
using ANTU.Models.RequestDto;
using ANTU.Models.Dto;
using ANTU.Resources.Rest.RestInterfaces;
using ANTU.Resources.Utilidades;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.IO;

namespace ANTU.Resources.Rest
{
    public class CatalogoProductoRest : ICatalogoProducto
    {
        internal readonly HttpClient httpClient;

        public CatalogoProductoRest(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> Add(CatalogoProductoRequestDto data, Func<Task> ejecutarTarea)
        {
            using StringContent json = new(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS_CATALOGPRODUCT[0], json);

            if (ejecutarTarea != null)
                await ejecutarTarea();

            if (httpResponse.IsSuccessStatusCode)
                await Mensaje.MensajeCorrecto("Guardado Exitosamente", await httpResponse.Content.ReadAsStringAsync());
            else
                await Mensaje.MensajeError("Error Guardado", await httpResponse.Content.ReadAsStringAsync());

            return (httpResponse.IsSuccessStatusCode) ? true : false;
        }

        public async Task<Dictionary<string, object>> SaveImages(ObservableCollection<FileResultExtensible> fileResultExtensible, string guid, bool activarVentanasAlerta = false, Func<Task>? ejecutarTask = null)
        {
            Dictionary<string, object> datos = new Dictionary<string, object>();
            MultipartFormDataContent multipartFormData = new();
            multipartFormData.Add(new StringContent(guid, Encoding.UTF8, MediaTypeNames.Text.Plain), "identificador");

            foreach (FileResultExtensible fileResult in fileResultExtensible)
            {
                var streamContent = new StreamContent(File.OpenRead(fileResult.FullPath));
                streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);

                multipartFormData.Add(streamContent, "formFiles", fileResult.FileName);
            }

            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS_CATALOGPRODUCT[1], multipartFormData);

            if (ejecutarTask != null)
                await ejecutarTask();

            if (httpResponse.IsSuccessStatusCode && activarVentanasAlerta)
            {
                RequestDataImage resultadoContenido = JsonConvert.DeserializeObject<RequestDataImage>(await httpResponse.Content.ReadAsStringAsync())!;
                await Mensaje.MensajeCorrecto("Subida Imagenes", resultadoContenido.mensaje);

                datos.Add("imagenes", resultadoContenido.imagenes);
            }
            else
                await Mensaje.MensajeError("Error Subida Imagenes", await httpResponse.Content.ReadAsStringAsync());

            datos.Add("estado", (httpResponse.IsSuccessStatusCode) ? true : false);

            return datos;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CatalogoProducto>> Get(object data)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}

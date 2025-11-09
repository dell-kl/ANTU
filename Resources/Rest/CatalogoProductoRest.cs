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
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ANTU.Resources.Rest
{
    public class CatalogoProductoRest : ICatalogoProducto
    {
        internal readonly HttpClient httpClient;

        public CatalogoProductoRest(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> Add(CatalogoProductoRequestDto data, Func<Task> ejecutarTarea, bool mostrarMensajes = false)
        {
            using StringContent json = new(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS_CATALOGPRODUCT[0], json);

            if (ejecutarTarea != null)
                await ejecutarTarea();

            if (httpResponse.IsSuccessStatusCode && mostrarMensajes)
                await Mensaje.MensajeCorrecto("Guardado Exitosamente", await httpResponse.Content.ReadAsStringAsync());
            else if(!httpResponse.IsSuccessStatusCode && mostrarMensajes)
                await Mensaje.MensajeError("Error Guardado", await httpResponse.Content.ReadAsStringAsync());

            return (httpResponse.IsSuccessStatusCode) ? true : false;
        }

        public async Task<bool> Add(CatalogoProductoRequestDto data, Func<Task> ejecutarTarea, ObservableCollection<FileResultExtensible> fileResultExtensibles)
        {
            if(!fileResultExtensibles.Any())
                return await this.Add(data, ejecutarTarea, true);

            bool resultado = await this.Add(data, async () => { }, false);
            Dictionary<string, object> resultadoImagenes = await SaveImages(fileResultExtensibles, data.identificador, activarVentanasAlerta: false);
            await ejecutarTarea();

            if ( resultado && resultadoImagenes.ContainsKey("estado") && resultadoImagenes["estado"] is true )
                await Mensaje.MensajeCorrecto("Guardado Exitosamente", "Producto e imagenes guardadas correctamente.");
            else if (resultado && resultadoImagenes.ContainsKey("estado") && resultadoImagenes["estado"] is false)
                await Mensaje.MensajeError("Guardado Incompleto", "Producto guardado correctamente, pero no se pudieron guardar las imagenes.");
            else
                await Mensaje.MensajeError("Error Guardado", "No se pudieron guardar los datos del producto nuevo.");
            
            return resultado;
        }

        public async Task<IEnumerable<CatalogoProducto>> Get(object data)
        {
            IEnumerable<CatalogoProducto> listado = new List<CatalogoProducto>();

            //Endpoints.ENDPOINTS_CATALOGPRODUCT[3] es el endpoint para obtener productos por categoria
            using HttpResponseMessage httpResponse = await httpClient.GetAsync($"{Endpoints.ENDPOINTS_CATALOGPRODUCT[3]}/{data}");

            if (httpResponse.IsSuccessStatusCode)
                listado = JsonConvert.DeserializeObject<IEnumerable<CatalogoProducto>>(await httpResponse.Content.ReadAsStringAsync())!;

            return listado;
        }

        //aqui crearemos otro metodo parecido al metodo GET implementado.
        public async Task<IEnumerable<DataCatalogProducto>> GetDataCatalogProducto(object data, string GuidCatalogProduct)
        {
            IEnumerable<DataCatalogProducto> listado = new List<DataCatalogProducto>();

            using HttpResponseMessage httpResponse = await httpClient.GetAsync($"{Endpoints.ENDPOINTS_CATALOGPRODUCT[4]}/{data}?guid={GuidCatalogProduct}");

            if (httpResponse.IsSuccessStatusCode)
                listado = JsonConvert.DeserializeObject<IEnumerable<DataCatalogProducto>>(await httpResponse.Content.ReadAsStringAsync())!;

            return listado;
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

            //El Endpoints.ENDPOINTS_CATALOGPRODUCT[2] es el endpoint para subir imagenes
            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS_CATALOGPRODUCT[2], multipartFormData);

            if (ejecutarTask != null)
                await ejecutarTask();

            if (httpResponse.IsSuccessStatusCode && activarVentanasAlerta)
            {
                RequestDataImage resultadoContenido = JsonConvert.DeserializeObject<RequestDataImage>(await httpResponse.Content.ReadAsStringAsync())!;
                await Mensaje.MensajeCorrecto("Subida Imagenes", resultadoContenido.mensaje);

                datos.Add("imagenes", resultadoContenido.imagenes);
            }
            else if (!httpResponse.IsSuccessStatusCode && activarVentanasAlerta)
            {
                string mensajeError = await httpResponse.Content.ReadAsStringAsync();
                await Mensaje.MensajeError("Error Subida Imagenes", mensajeError);
            }

            datos.Add("estado", (httpResponse.IsSuccessStatusCode) ? true : false);

            return datos;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }


    }
}

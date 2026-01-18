using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using Data.Rest.RestInterfaces;
// using ANTU.Resources.Utilidades;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace Data.Rest
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

            // if (httpResponse.IsSuccessStatusCode && mostrarMensajes)
            //     await _mensaje.MensajeCorrecto("Guardado Exitosamente", await httpResponse.Content.ReadAsStringAsync());
            // else if(!httpResponse.IsSuccessStatusCode && mostrarMensajes)
            //     await _mensaje.MensajeError("Error Guardado", await httpResponse.Content.ReadAsStringAsync());

            return (httpResponse.IsSuccessStatusCode) ? true : false;
        }

        public async Task<bool> Add(CatalogoProductoRequestDto data, Func<Task> ejecutarTarea, ObservableCollection<FileResultExtensible> fileResultExtensibles)
        {
            if(!fileResultExtensibles.Any())
                return await this.Add(data, ejecutarTarea, true);

            bool resultado = await this.Add(data, async () => { }, false);
            Dictionary<string, object> resultadoImagenes = await SaveImages(fileResultExtensibles, data.identificador, activarVentanasAlerta: false);
            await ejecutarTarea();

            // if ( resultado && resultadoImagenes.ContainsKey("estado") && resultadoImagenes["estado"] is true )
            //     await _mensaje.MensajeCorrecto("Guardado Exitosamente", "Producto e imagenes guardadas correctamente.");
            // else if (resultado && resultadoImagenes.ContainsKey("estado") && resultadoImagenes["estado"] is false)
            //     await _mensaje.MensajeError("Guardado Incompleto", "Producto guardado correctamente, pero no se pudieron guardar las imagenes.");
            // else
            //     await _mensaje.MensajeError("Error Guardado", "No se pudieron guardar los datos del producto nuevo.");
            
            return resultado;
        }

        public async Task<IEnumerable<CatalogoProducto>> Get(object data, Func<Task>? ejecutarTarea = null)
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
                // await _mensaje.MensajeCorrecto("Subida Imagenes", resultadoContenido.mensaje);

                datos.Add("imagenes", resultadoContenido.imagenes);
            }
            else if (!httpResponse.IsSuccessStatusCode && activarVentanasAlerta)
            {
                string mensajeError = await httpResponse.Content.ReadAsStringAsync();
                // await _mensaje.MensajeError("Error Subida Imagenes", mensajeError);
            }

            datos.Add("estado", (httpResponse.IsSuccessStatusCode) ? true : false);

            return datos;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(CatalogoProductoRequestDto data, Func<Task> ejecutarTarea)
        {
            //utilizaremos este metodo para poder hacer la actualizacion de los respectivos datos.
            using StringContent json = new(
            JsonConvert.SerializeObject(data),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

            using HttpResponseMessage httpResponse = await httpClient.PutAsync(Endpoints.ENDPOINTS_CATALOGPRODUCT[5], json);

            await ejecutarTarea();

            // if(httpResponse.IsSuccessStatusCode)
            //     await _mensaje.MensajeCorrecto("Actualizacion Exitosa", await httpResponse.Content.ReadAsStringAsync());
            // else
            //     await _mensaje.MensajeError("Error Actualizacion", await httpResponse.Content.ReadAsStringAsync());

            return httpResponse.IsSuccessStatusCode ? true : false;
        }

        public async Task<bool> AddDatosVentaDataCatalogProduct(CatalogoProductoRequestDto catalogProductRequestDto, Func<Task> ejecutarTarea)
        {
            using StringContent json = new(
            JsonConvert.SerializeObject(catalogProductRequestDto),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS_CATALOGPRODUCT[1], json);

            await ejecutarTarea();

            // if (httpResponse.IsSuccessStatusCode)
            //     await _mensaje.MensajeCorrecto("Datos Agregados", await httpResponse.Content.ReadAsStringAsync());
            // else if (httpResponse.StatusCode is HttpStatusCode.InternalServerError)
            //     await _mensaje.MensajeError("Error Servidor", await httpResponse.Content.ReadAsStringAsync());
            // else if (httpResponse.StatusCode is HttpStatusCode.NotFound)
            //     await _mensaje.MensajeError("Producto Inexistente", await httpResponse.Content.ReadAsStringAsync());
            // else
            //     await _mensaje.MensajeError("Error Inesperado", "Ocurrio un error al guardar tus datos, intentalo en otro momento");

            return (httpResponse.IsSuccessStatusCode) ? true : false;
        }

        public async Task<CatalogoProductoDetalle> GetDataCatalogProductoDetalle(string GuidCatalogProduct)
        {
            CatalogoProductoDetalle detalle = new CatalogoProductoDetalle();

            //Endpoints.ENDPOINTS_CATALOGPRODUCT[3] es el endpoint para obtener productos por categoria
            using HttpResponseMessage httpResponse = await httpClient.GetAsync($"{Endpoints.ENDPOINTS_CATALOGPRODUCT[6]}/{GuidCatalogProduct}");

            if (httpResponse.IsSuccessStatusCode)
                detalle = JsonConvert.DeserializeObject<CatalogoProductoDetalle>(await httpResponse.Content.ReadAsStringAsync())!;

            return detalle;
        }

        public async Task<bool> DeleteImages(ICollection<DataImage> dataImages, Func<Task>? ejecutarTask = null)
        {
            using StringContent json = new(
                JsonConvert.SerializeObject(dataImages),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS_CATALOGPRODUCT[7], json);

            if (httpResponse != null)
                await ejecutarTask();

            // if (httpResponse.IsSuccessStatusCode)
            //     await _mensaje.MensajeCorrecto("Eliminar Imagenes", await httpResponse.Content.ReadAsStringAsync());
            // else
            //     await _mensaje.MensajeError("Error Eliminar Imagenes", await httpResponse.Content.ReadAsStringAsync());

            return (httpResponse.IsSuccessStatusCode) ? true : false;
        }
    }
}

using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using Data.Rest.RestInterfaces;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Modelos.ResultDto;

namespace Data.Rest
{
    public class CatalogoProductoRest : ICatalogoProducto
    {
        internal readonly HttpClient httpClient;
        
        public CatalogoProductoRest(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<RequestResultDto<object>> Add(CatalogoProductoRequestDto data)
        {
            try
            {
                using StringContent json = new(
                    JsonConvert.SerializeObject(data),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);

                using HttpResponseMessage httpResponse =
                    await httpClient.PostAsync(Endpoints.ENDPOINTS_CATALOGPRODUCT[0], json);

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<object>(await httpResponse.Content.ReadAsStringAsync(), httpResponse.StatusCode);

                return Result.Success<object>(await httpResponse.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                return Result.Failure<object>("Se ha perdido la conexion al servicio", HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                return Result.Failure<object>("No se puedo procesar la solicitud para registrar tu nuevo catalogo de producto", HttpStatusCode.BadRequest);
            }
        }

        public Task<RequestResultDto<string>> Add(CatalogoProductoRequestDto data, ObservableCollection<FileResultExtensible> fileResultExtensibles)
        {
            throw new NotImplementedException();
        }

        public async Task<RequestResultDto<IEnumerable<CatalogoProducto>>> Get(object data)
        {
            try
            {
                //Endpoints.ENDPOINTS_CATALOGPRODUCT[3] es el endpoint para obtener productos por categoria
                using HttpResponseMessage httpResponse =
                    await httpClient.GetAsync($"{Endpoints.ENDPOINTS_CATALOGPRODUCT[3]}/{data}");

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<IEnumerable<CatalogoProducto>>(await httpResponse.Content.ReadAsStringAsync(),
                        httpResponse.StatusCode);

                IEnumerable<CatalogoProducto> listado =
                    JsonConvert.DeserializeObject<IEnumerable<CatalogoProducto>>(
                        await httpResponse.Content.ReadAsStringAsync())!;

                return Result.Success(listado);
            }
            catch (HttpRequestException)
            {
                return Result.Failure<IEnumerable<CatalogoProducto>>("Conexin perdida, no se pudieron traer mas datos",
                    HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                return Result.Failure<IEnumerable<CatalogoProducto>>("Hubo un error en procesar los datos del servidor.",
                    HttpStatusCode.InternalServerError);
            }
        }
        
        public async Task<IEnumerable<DataCatalogProducto>> GetDataCatalogProducto(object data, string GuidCatalogProduct)
        {
            IEnumerable<DataCatalogProducto> listado = new List<DataCatalogProducto>();

            using HttpResponseMessage httpResponse = await httpClient.GetAsync($"{Endpoints.ENDPOINTS_CATALOGPRODUCT[4]}/{data}?guid={GuidCatalogProduct}");

            if (httpResponse.IsSuccessStatusCode)
                listado = JsonConvert.DeserializeObject<IEnumerable<DataCatalogProducto>>(await httpResponse.Content.ReadAsStringAsync())!;

            return listado;
        }
        
        
        //aqui crearemos otro metodo parecido al metodo GET implementado.
        public  async Task<RequestResultDto<object>> RegistrarImagenesCatalogoProducto(ObservableCollection<FileResultExtensible> fileResultExtensibles, string guid)
        {
            MultipartFormDataContent multipartFormData = new();
            List<FileStream> archivosAbiertos = new();
            try
            {
                Dictionary<string, object> datos = new Dictionary<string, object>();
                multipartFormData.Add(new StringContent(guid, Encoding.UTF8, MediaTypeNames.Text.Plain),
                    "identificador");

                foreach (FileResultExtensible fileResult in fileResultExtensibles)
                {
                    FileStream leyendoArchivo = File.OpenRead(fileResult.FullPath!);
                    archivosAbiertos.Add(leyendoArchivo);

                    var streamContent = new StreamContent(leyendoArchivo);
                    streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);

                    multipartFormData.Add(streamContent, "formFiles", fileResult.FileName);
                }

                using HttpResponseMessage httpResponse =
                    await httpClient.PostAsync(Endpoints.ENDPOINTS_CATALOGPRODUCT[2], multipartFormData);

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<object>(await httpResponse.Content.ReadAsStringAsync(), httpResponse.StatusCode);
                
                RequestDataImage resultadoContenido = JsonConvert.DeserializeObject<RequestDataImage>(await httpResponse.Content.ReadAsStringAsync())!;
                
                return Result.Success<object>(resultadoContenido);
            }
            catch (HttpRequestException)
            {
                return Result.Failure<object>("Conexion perdida, no se pudieron registrar las imagenes", HttpStatusCode.RequestTimeout);
            }
            catch (Exception e)
            {
                return Result.Failure<object>("Hubo un error en procesar la solicitud de subida de imagenes", HttpStatusCode.BadRequest);
            }
            finally
            {
                foreach(FileStream archivo in archivosAbiertos)
                    await archivo.DisposeAsync();
                
                multipartFormData.Dispose();
            }
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

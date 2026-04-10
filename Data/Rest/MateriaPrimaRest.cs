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
    public class MateriaPrimaRest : IMateriaPrima
    {
        internal readonly HttpClient httpClient;
        
        public MateriaPrimaRest(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<RequestResultDto<object>> Add(MateriaPrimaRequestDto materiaPrimaRequestDto)
        {
            try
            {
                using StringContent json = new(
                    JsonConvert.SerializeObject(materiaPrimaRequestDto),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);

                using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.MateriaPrimaRutas.RegistrarMateriaPrima, json);

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<object>(await httpResponse.Content.ReadAsStringAsync(), httpResponse.StatusCode);

                return Result.Success<object>(await httpResponse.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                return Result.Failure<object>("Se ha perdido la conexion al servicio", HttpStatusCode.RequestTimeout);
            }
            catch (Exception e)
            {
                return Result.Failure<object>("No se puedo procesar la solicitud para registrar materia prima", HttpStatusCode.BadRequest);
            }
        }

        //Realizando modificaciones a este codigo. Por el momento que comentado para posible actualizaciones.
        public async Task<RequestResultDto<string>> Add(MateriaPrimaRequestDto data, ObservableCollection<FileResultExtensible> fileResultExtensibles)
        {
            throw new NotImplementedException();
        }


        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<RequestResultDto<IEnumerable<MateriaPrimaProducto>>> Get(object data)
        {
            try
            {
                using HttpResponseMessage? httpResponse = await httpClient.GetAsync($"{Endpoints.MateriaPrimaRutas.SolicitarMateriaPrima}/{data}");

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<IEnumerable<MateriaPrimaProducto>>(
                        await httpResponse.Content.ReadAsStringAsync(), httpResponse.StatusCode);

                IEnumerable<MateriaPrimaProducto> listMateriaPrima =
                    JsonConvert.DeserializeObject<IEnumerable<MateriaPrimaProducto>>(
                        await httpResponse.Content.ReadAsStringAsync())!;
                return Result.Success<IEnumerable<MateriaPrimaProducto>>(listMateriaPrima);
            }
            catch (HttpRequestException)
            {
                return Result.Failure<IEnumerable<MateriaPrimaProducto>>("Conexion perdida, no se pudieron traer mas datos", HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                return Result.Failure<IEnumerable<MateriaPrimaProducto>>("Hubo un error en procesar los datos del servidor.", HttpStatusCode.InternalServerError);
            }
        }
        
        public async Task<RequestResultDto<object>> RegistrarImagenesMateriaPrima(ObservableCollection<FileResultExtensible> fileResultExtensibles, string guid)
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
                    await httpClient.PostAsync(Endpoints.MateriaPrimaRutas.RegistrarImagenes, multipartFormData);

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

        public async Task<RequestResultDto<bool>> DeleteImages(ICollection<DataImage> dataImages)
        {
            try
            {
                using StringContent json = new(
                    JsonConvert.SerializeObject(dataImages),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);

                using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.MateriaPrimaRutas.EliminarImagenesMateriaPrima, json);

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<bool>(await httpResponse.Content.ReadAsStringAsync(),
                        httpResponse.StatusCode);

                return Result.Success(true);
            }
            catch (HttpRequestException)
            {
                return Result.Failure<bool>("Conexion perdida, no se pudieron eliminar las imagenes", HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                return Result.Failure<bool>("Error del servidor para eliminar las imagenes", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<bool> Update(MateriaPrimaRequestDto data, Func<Task> ejecutarTarea)
        {
            return false;
        }

        public async Task<RequestResultDto<MateriaPrimaDetalle>> MateriaPrimaDetalles(string guid)
        {
            try
            {
                using HttpResponseMessage httpResponse = await httpClient.GetAsync($"{Endpoints.MateriaPrimaRutas.DetalleMateriaPrima}/{guid}");

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<MateriaPrimaDetalle>(await httpResponse.Content.ReadAsStringAsync(),
                        httpResponse.StatusCode);

                MateriaPrimaDetalle materiaPrimaDetalle =
                    JsonConvert.DeserializeObject<MateriaPrimaDetalle>(await httpResponse.Content.ReadAsStringAsync())!;

                return Result.Success(materiaPrimaDetalle);
            }
            catch (HttpRequestException e)
            {
                return Result.Failure<MateriaPrimaDetalle>("Se perdio la conexion al servicio, intentalo en otro momento.",
                    HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                return Result.Failure<MateriaPrimaDetalle>("Error interno del servidor al procesar los datos", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<RequestResultDto<KgSeguimiento>> AgregarStockMateriaPrima(StockMateriaPrimaRequestDto stockMateriaPrima)
        {
            try
            {
                using StringContent stringContenido = new(
                    JsonConvert.SerializeObject(stockMateriaPrima),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json
                );

                using HttpResponseMessage httpResponse =
                    await httpClient.PostAsync(Endpoints.MateriaPrimaRutas.AgregarEnStock, stringContenido);

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<KgSeguimiento>(await httpResponse.Content.ReadAsStringAsync(),
                        httpResponse.StatusCode);

                KgSeguimiento resultado = JsonConvert.DeserializeObject<KgSeguimiento>(await httpResponse.Content.ReadAsStringAsync())!;
                
                return Result.Success(resultado);
            }
            catch (HttpRequestException)
            {
                return Result.Failure<KgSeguimiento>("Conexion perdida, no se pudo enviar los datos", HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                return Result.Failure<KgSeguimiento>("Error del servidor para procesar los datos", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<RequestResultDto<bool>> EditarDatosMateriaPrima(MateriaPrimaRequestDto materiaPrimaRequestDTO)
        {
            try
            {
                using StringContent stringContent = new(
                    JsonConvert.SerializeObject(materiaPrimaRequestDTO),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json
                );
                using HttpResponseMessage httpResponse =
                    await httpClient.PutAsync(Endpoints.MateriaPrimaRutas.EditarNombreMateriaPrima, stringContent);

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<bool>(await httpResponse.Content.ReadAsStringAsync(),
                        httpResponse.StatusCode);

                return Result.Success(true);
            }
            catch (HttpRequestException)
            {
                return Result.Failure<bool>("Error de conexion con el servidor, no se pudieron editar los datos",
                    HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                return Result.Failure<bool>("Error del servidor para procesar los datos", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<RequestResultDto<IEnumerable<KgSeguimiento>>> GetKgSeguimientos(MateriaPrimaDetalle datos)
        {
            try
            {
                IEnumerable<KgSeguimiento> listadoKgSeguimientos = new List<KgSeguimiento>();

                using HttpResponseMessage httpResponse = await httpClient.GetAsync(
                    $"{Endpoints.MateriaPrimaRutas.SolicitarKgMonitoring}/{datos!.NValoresListadoKgSeguimiento}?guid={datos!.Identificador}");

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<IEnumerable<KgSeguimiento>>(
                        "No se pudieron obtener los datos de seguimientos sobre tus compras", httpResponse.StatusCode);

                listadoKgSeguimientos = JsonConvert.DeserializeObject<IEnumerable<KgSeguimiento>>(await httpResponse.Content.ReadAsStringAsync())!;
                
                return Result.Success(listadoKgSeguimientos);
            }
            catch (HttpRequestException)
            {
                return Result.Failure<IEnumerable<KgSeguimiento>>(
                    "Se perdio la conexion al servicio, intentalo en otro momento.",
                    HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                return Result.Failure<IEnumerable<KgSeguimiento>>("Error interno del servidor al procesar los datos", HttpStatusCode.InternalServerError);
            }
        }
    }
}

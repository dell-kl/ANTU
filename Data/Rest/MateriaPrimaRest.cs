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

        public async Task<RequestResultDto<string>> Add(MateriaPrimaRequestDto materiaPrimaRequestDto)
        {
            try
            {
                using StringContent json = new(
                    JsonConvert.SerializeObject(materiaPrimaRequestDto),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);

                using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS[0], json);

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<string>(await httpResponse.Content.ReadAsStringAsync(), httpResponse.StatusCode);

                return Result.Success<string>(await httpResponse.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException)
            {
                return Result.Failure<string>("Se ha perdido la conexion al servicio", HttpStatusCode.RequestTimeout);
            }
            catch (Exception e)
            {
                return Result.Failure<string>("No se puedo procesar la solicitud para registrar materia prima", HttpStatusCode.BadRequest);
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
            IEnumerable<MateriaPrimaProducto> listMateriaPrima = new List<MateriaPrimaProducto>();
            
            using HttpResponseMessage? httpResponse =  await httpClient.GetAsync($"{Endpoints.ENDPOINTS[2]}/{data}");

            if (httpResponse.IsSuccessStatusCode)
                listMateriaPrima =
                    JsonConvert.DeserializeObject<IEnumerable<MateriaPrimaProducto>>(
                        await httpResponse.Content.ReadAsStringAsync())!;
            
            return null;
        }
        
        public async Task<Dictionary<string, object>> SaveImages(ObservableCollection<FileResultExtensible> fileResultExtensible, string guid, bool activarVentanasAlerta = false, Func<Task>? ejecutarTask = null)
        {
            Dictionary<string, object> datos = new Dictionary<string, object>();
            MultipartFormDataContent multipartFormData = new();
            multipartFormData.Add(new StringContent(guid, Encoding.UTF8, MediaTypeNames.Text.Plain), "identificador");

            foreach(FileResultExtensible fileResult in fileResultExtensible)
            {
                var streamContent = new StreamContent(File.OpenRead(fileResult.FullPath));
                streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);

                multipartFormData.Add(streamContent, "formFiles", fileResult.FileName);
            }
            
            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS[1], multipartFormData);

            if (ejecutarTask != null)
                await ejecutarTask();

            if (httpResponse.IsSuccessStatusCode && activarVentanasAlerta)
            {
                RequestDataImage resultadoContenido = JsonConvert.DeserializeObject<RequestDataImage>(await httpResponse.Content.ReadAsStringAsync())!;
                // await _mensaje.MensajeCorrecto("Subida Imagenes", resultadoContenido.mensaje);

                datos.Add("imagenes", resultadoContenido.imagenes);
            }
            // else if (httpResponse.StatusCode == HttpStatusCode.InternalServerError && activarVentanasAlerta)
            //     await _mensaje.MensajeError("Error Subida Imagenes", await httpResponse.Content.ReadAsStringAsync());

            datos.Add("estado", (httpResponse.IsSuccessStatusCode) ? true : false);

            return datos;
        }

        public async Task<RequestResultDto<string>> RegistrarImagenesMateriaPrima(ObservableCollection<FileResultExtensible> fileResultExtensibles, string guid)
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
                    await httpClient.PostAsync(Endpoints.ENDPOINTS[1], multipartFormData);

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return Result.Failure<string>(await httpResponse.Content.ReadAsStringAsync(), httpResponse.StatusCode);
                
                RequestDataImage resultadoContenido = JsonConvert.DeserializeObject<RequestDataImage>(await httpResponse.Content.ReadAsStringAsync())!;
                
                return Result.Success(resultadoContenido.mensaje);
            }
            catch (HttpRequestException)
            {
                return Result.Failure<string>("Conexion perdida, no se pudieron registrar las imagenes", HttpStatusCode.RequestTimeout);
            }
            catch (Exception e)
            {
                return Result.Failure<string>("Hubo un error en procesar la solicitud de subida de imagenes", HttpStatusCode.BadRequest);
            }
            finally
            {
                foreach(FileStream archivo in archivosAbiertos)
                    await archivo.DisposeAsync();
                
                multipartFormData.Dispose();
            }
        }

        public async Task<bool> DeleteImages(ICollection<DataImage> dataImages, Func<Task>? ejecutarTask = null)
        {
            using StringContent json = new(
                JsonConvert.SerializeObject(dataImages),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS[7], json);

            if (httpResponse != null)
                await ejecutarTask();

            // if (httpResponse.IsSuccessStatusCode)
            //     await _mensaje.MensajeCorrecto("Eliminar Imagenes", await httpResponse.Content.ReadAsStringAsync());
            // else
            //     await _mensaje.MensajeError("Error Eliminar Imagenes", await httpResponse.Content.ReadAsStringAsync());
            
            return (httpResponse.IsSuccessStatusCode) ? true : false;
        }

        public async Task<bool> Update(MateriaPrimaRequestDto data, Func<Task> ejecutarTarea)
        {
            return false;
        }

        public async Task<RequestResultDto<MateriaPrimaDetalle>> MateriaPrimaDetalles(string guid)
        {
            try
            {
                using HttpResponseMessage httpResponse = await httpClient.GetAsync($"{Endpoints.ENDPOINTS[3]}/{guid}");

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

        public async Task<bool> AgregarStockMateriaPrima(StockMateriaPrimaRequestDto stockMateriaPrima, Func<Task>? ejecutarTask = null)
        {
            using StringContent stringContenido = new(
                JsonConvert.SerializeObject(stockMateriaPrima),
                Encoding.UTF8,
                MediaTypeNames.Application.Json
            );

            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS[4], stringContenido);

            if (ejecutarTask != null)
                await ejecutarTask();

            // if (httpResponse.IsSuccessStatusCode)
            //     await _mensaje.MensajeCorrecto("Agregar stock materia prima", await httpResponse.Content.ReadAsStringAsync());
            // else
            //     await _mensaje.MensajeError("Error Agregar stock materia prima", await httpResponse.Content.ReadAsStringAsync());

            return (httpResponse.IsSuccessStatusCode) ? true : false;
        }

        public async Task<bool> EditarDatosMateriaPrima(MateriaPrimaRequestDto materiaPrimaRequestDTO, Func<Task>? ejecutarTask = null)
        {
            using StringContent stringContent = new(
                JsonConvert.SerializeObject(materiaPrimaRequestDTO),
                Encoding.UTF8,
                MediaTypeNames.Application.Json
            );
            using HttpResponseMessage httpResponse = await httpClient.PutAsync(Endpoints.ENDPOINTS[5], stringContent);

            if (ejecutarTask != null)
                await ejecutarTask();

            // if (httpResponse.IsSuccessStatusCode)
            //     await _mensaje.MensajeCorrecto("Editar Materia Prima", await httpResponse.Content.ReadAsStringAsync());
            // else
            //     await _mensaje.MensajeError("Error Materia Prima", await httpResponse.Content.ReadAsStringAsync());

            return (httpResponse.IsSuccessStatusCode) ? true : false;
        }

        public async Task<RequestResultDto<IEnumerable<KgSeguimiento>>> GetKgSeguimientos(MateriaPrimaDetalle datos)
        {
            try
            {
                IEnumerable<KgSeguimiento> listadoKgSeguimientos = new List<KgSeguimiento>();

                using HttpResponseMessage httpResponse = await httpClient.GetAsync(
                    $"{Endpoints.ENDPOINTS[8]}/{datos!.NValoresListadoKgSeguimiento}?guid={datos!.Identificador}");

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

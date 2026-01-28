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

namespace Data.Rest
{
    public class MateriaPrimaRest : IMateriaPrima
    {
        internal readonly HttpClient httpClient;
        
        public MateriaPrimaRest(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> Add(MateriaPrimaRequestDto materiaPrimaRequestDto, Func<Task> ejecutarTarea, bool mostrarMensajes = false)
        {
            using StringContent json = new(
                JsonConvert.SerializeObject(materiaPrimaRequestDto),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS[0], json);

            await ejecutarTarea();

            // if (httpResponse.IsSuccessStatusCode && mostrarMensajes) 
            //     await _mensaje.MensajeCorrecto("Guardado Exitosamente", await httpResponse.Content.ReadAsStringAsync());
            // else if (httpResponse.StatusCode == HttpStatusCode.InternalServerError && mostrarMensajes)
            //     await _mensaje.MensajeError("Error Guardado", await httpResponse.Content.ReadAsStringAsync());

            return (httpResponse.IsSuccessStatusCode) ? true : false;
        }

        public async Task<bool> Add(MateriaPrimaRequestDto data, Func<Task> ejecutarTarea, ObservableCollection<FileResultExtensible> fileResultExtensibles)
        {
            if (!fileResultExtensibles.Any())
                return await this.Add(data, ejecutarTarea, true);

            bool resultado = await this.Add(data, async () => { }, false);
            Dictionary<string, object> resultadoImagenes = await SaveImages(fileResultExtensibles, data.id_dto!, activarVentanasAlerta: false);
            await ejecutarTarea();

            // if (resultado && resultadoImagenes["estado"] is true)
            //     await _mensaje.MensajeCorrecto("Guardado Exitosamente", "Materia prima guardada correctamente.");
            // else if (resultado && resultadoImagenes["estado"] is false)
            //     await _mensaje.MensajeError("Guardado Incompleto", "Materia prima guardado correctamente, pero no se pudieron guardar las imagenes.");
            // else
            //     await _mensaje.MensajeError("Error Guardado", "No se pudieron guardar los datos de la materia prima.");

            return resultado;
        }


        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MateriaPrimaProducto>> Get(object data, Func<Task>? ejecutarTarea = null)
        {
            IEnumerable<MateriaPrimaProducto> listMateriaPrima = new List<MateriaPrimaProducto>();
            
            using HttpResponseMessage? httpResponse =  await httpClient.GetAsync($"{Endpoints.ENDPOINTS[2]}/{data}");

            if (httpResponse.IsSuccessStatusCode)
                listMateriaPrima =
                    JsonConvert.DeserializeObject<IEnumerable<MateriaPrimaProducto>>(
                        await httpResponse.Content.ReadAsStringAsync())!;
            
            return listMateriaPrima;
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

        public async Task<MateriaPrimaDetalle> MateriaPrimaDetalles(string guid)
        {
            MateriaPrimaDetalle materiaPrimaDetalle = new MateriaPrimaDetalle();
            using HttpResponseMessage httpResponse = await httpClient.GetAsync($"{Endpoints.ENDPOINTS[3]}/{guid}");

            if ( httpResponse.IsSuccessStatusCode )
            {
                materiaPrimaDetalle = JsonConvert.DeserializeObject<MateriaPrimaDetalle>(await httpResponse.Content.ReadAsStringAsync())!;
            }

            return materiaPrimaDetalle;
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

        public async Task<IEnumerable<KgSeguimiento>> GetKgSeguimientos(object data, string guidMateriaPrima)
        {
            IEnumerable<KgSeguimiento> listadoKgSeguimientos = new List<KgSeguimiento>();

            using HttpResponseMessage httpResponse = await httpClient.GetAsync($"{Endpoints.ENDPOINTS[8]}/{data}?guid={guidMateriaPrima}");

            if (httpResponse.IsSuccessStatusCode)
                listadoKgSeguimientos = JsonConvert.DeserializeObject<IEnumerable<KgSeguimiento>>(await httpResponse.Content.ReadAsStringAsync())!;

            return listadoKgSeguimientos;
        }
    }
}

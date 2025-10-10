using ANTU.Models;
using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Rest.RestInterfaces;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ANTU.Resources.Rest
{
    public class MateriaPrimaRest : IMateriaPrima
    {
        private readonly IHttpClientFactory _httpClientFactory;
        internal readonly HttpClient httpClient;
        public MateriaPrimaRest(IHttpClientFactory httpClientFactory )
        {
            _httpClientFactory = httpClientFactory;

            httpClient = _httpClientFactory.CreateClient("HttpClientRest");
        }

        public async Task<bool> Add(MateriaPrimaRequestDto materiaPrimaRequestDto)
        {

            using StringContent json = new(
                JsonConvert.SerializeObject(materiaPrimaRequestDto),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            try
            {
                using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS[0], json);

                if (httpResponse.IsSuccessStatusCode) {
                    return true;
                }
            }
            catch(Exception e)
            {
                return false;
            }

            return false;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MateriaPrimaProducto>> Get(object data)
        {
            IEnumerable<MateriaPrimaProducto> listMateriaPrima = new List<MateriaPrimaProducto>();

            using HttpResponseMessage httpResponse =  await httpClient.GetAsync($"{Endpoints.ENDPOINTS[2]}/{data}");
            
            if (httpResponse.IsSuccessStatusCode)
                listMateriaPrima = JsonConvert.DeserializeObject<IEnumerable<MateriaPrimaProducto>>(await httpResponse.Content.ReadAsStringAsync())!;

            return listMateriaPrima;
        }

        public async Task<bool> SaveImages(ObservableCollection<FileResultExtensible> fileResultExtensible, string guid)
        {
            MultipartFormDataContent multipartFormData = new();
            multipartFormData.Add(new StringContent(guid, Encoding.UTF8, MediaTypeNames.Text.Plain), "identificador");

            foreach(FileResultExtensible fileResult in fileResultExtensible)
            {
                var streamContent = new StreamContent(File.OpenRead(fileResult.FullPath));
                streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);

                multipartFormData.Add(streamContent, "formFiles", fileResult.FileName);
            }

            try
            {
                using HttpResponseMessage httpResponse = await httpClient.PostAsync(Endpoints.ENDPOINTS[1], multipartFormData);

                if (httpResponse.IsSuccessStatusCode) 
                    return true;
            }
            catch(Exception ex)
            {
                return false;
            }

            return false;
        }

        public async void Update()
        {
            
        }

    }
}

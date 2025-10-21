using ANTU.Resources.Rest.RestInterfaces;

namespace ANTU.Resources.Rest
{
    public class RestManagement : IRestManagement
    {
        public IMateriaPrima MateriaPrima { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClient httpClient { set; get; }

        public RestManagement(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            httpClient = _httpClientFactory.CreateClient("HttpClientRest");

            this.MateriaPrima = new MateriaPrimaRest(httpClient);
        }

    }
}

using ANTU.Resources.Rest.RestInterfaces;

namespace ANTU.Resources.Rest
{
    public class RestManagement : IRestManagement
    {
        public IMateriaPrima MateriaPrima { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public RestManagement(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            this.MateriaPrima = new MateriaPrimaRest(_httpClientFactory);
        }
    }
}

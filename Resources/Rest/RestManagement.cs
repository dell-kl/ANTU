using ANTU.Resources.Rest.RestInterfaces;

namespace ANTU.Resources.Rest
{
    public class RestManagement : IRestManagement
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClient httpClient { set; get; }
        
        
        public IMateriaPrima MateriaPrima { get; set; }

        public ICatalogoProducto CatalogoProduct { set; get; }
        
        public IProduccion Produccion { set; get; }

        public IFabricado Fabricado { set; get; }
        
        public IProduccionLista ProduccionLista { set; get; }
        
        
        public RestManagement(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            httpClient = _httpClientFactory.CreateClient("HttpClientRest");

            this.MateriaPrima = new MateriaPrimaRest(httpClient);
            this.CatalogoProduct = new CatalogoProductoRest(httpClient);
            this.Produccion = new ProduccionRest(httpClient);
            this.Fabricado = new FabricadoRest(httpClient);
            this.ProduccionLista = new ProduccionListaRest(httpClient);
        }

    }
}

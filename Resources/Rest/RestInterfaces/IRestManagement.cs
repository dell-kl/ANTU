namespace ANTU.Resources.Rest.RestInterfaces
{
    public interface IRestManagement
    {
        public IMateriaPrima MateriaPrima { get; }

        public ICatalogoProducto CatalogoProduct { get;  }

        public HttpClient httpClient { set; get; }
    }
}

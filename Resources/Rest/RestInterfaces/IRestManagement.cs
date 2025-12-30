namespace ANTU.Resources.Rest.RestInterfaces
{
    public interface IRestManagement
    {
        public IMateriaPrima MateriaPrima { get; }

        public ICatalogoProducto CatalogoProduct { get;  }

        public IProduccion Produccion { get;  }
        
        public IFabricado Fabricado { get;  }
        
        public HttpClient httpClient { set; get; }
    }
}

namespace Data.Rest.RestInterfaces
{
    public interface IRestManagement
    {
        public IMateriaPrima MateriaPrima { get; }

        public ICatalogoProducto CatalogoProduct { get;  }

        public IProduccion Produccion { get;  }
        
        public IFabricado Fabricado { get;  }
        
        public IProduccionLista ProduccionLista { get;  }
        
        public HttpClient httpClient { set; get; }
        
    }
}

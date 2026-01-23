using Business.Services.IServices;
using Data.Rest.RestInterfaces;

namespace Business.Services;

public class ManagementService : IManagementService
{
    public IMateriaPrimaService materiaPrimaService { get; private set; }
    public ICatalogoProductoService CatalogoProductoService { get; private set; }
    public IFabricacionService FabricacionService { get; private set;  }
    public IProductosListosService ProductosListosService { get; private set;  }

    public ManagementService(IRestManagement restManagement)
    {
        this.materiaPrimaService = new MateriaPrimaService(restManagement);
        this.CatalogoProductoService = new CatalogoProductoService(restManagement);
        this.FabricacionService = new FabricacionService(restManagement);
        this.ProductosListosService = new ProductosListosService(restManagement);
    }
}
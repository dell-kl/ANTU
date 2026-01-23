namespace Business.Services.IServices;

public interface IManagementService
{
    public IMateriaPrimaService materiaPrimaService { get; }
    public ICatalogoProductoService CatalogoProductoService { get; }
    public IFabricacionService FabricacionService { get; }
    public IProductosListosService ProductosListosService { get; }
}
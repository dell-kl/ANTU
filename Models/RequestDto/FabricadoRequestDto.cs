namespace ANTU.Models.RequestDto;

public class FabricadoRequestDto
{
    public string identificadorDataCatalogProduction { set; get; } = null!;
    public string identificadorProduccion { set; get; } = null!;
    public int cantidadCostales { set; get; } = 1;
}
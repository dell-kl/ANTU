namespace Modelos.RequestDto;

public class ProduccionReqestDto
{
    public string catalogoIdentificador { set; get; } = null!;

    public int numeroVecesProduccion { set; get; } = 1;
        
    public string nombreProduccion { set; get; } = null!;
    
    public ICollection<MaterialProduccion> materialesProduccion { set; get; } = new List<MaterialProduccion>();
}

public class MaterialProduccion
{
    public string id_dto { set; get; } = Guid.NewGuid().ToString();

    public double KgUse_dto { set; get; }
}
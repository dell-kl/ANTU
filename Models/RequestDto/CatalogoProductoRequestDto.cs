namespace ANTU.Models.RequestDto
{
    public class CatalogoProductoRequestDto
    {
        public string identificador { set; get; } = "";

        public string nombreProducto { set; get; } = "sin_nombre";

        public ICollection<DataProduct> dataCatalogProducts { set; get; } = new List<DataProduct>();
    }

    public class DataProduct
    {
        public decimal precio { set; get; }

        public double pesoKg { set; get; }

        public int cantidadTotal { set; get; }
    }
}

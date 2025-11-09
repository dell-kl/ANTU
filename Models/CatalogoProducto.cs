namespace ANTU.Models
{
    /*
     Dentro de este fichero definiremos algunas clases mas que estan relacionadas con lo que es 
    nuestro Catalog Producto.
     */
    public class CatalogoProducto
    {
        public string guid { set; get; } = Guid.NewGuid().ToString();
        public string nombreProducto { set; get; } = null!;
        public string rutaImagen { set; get; } = null!;
        public DateTime fechaCreacion { set; get; } = DateTime.Now;
        public int numeroCategorias { set; get; } = 0;
        public int totalSacosCatalogo { set; get; } = 0;
    }

    public class DataCatalogProducto {
        public string guid { set; get; } = "";
        public DateTime fechaCreacion { set; get; } = DateTime.Now;
        public DateTime fechaActualizacion { set; get; } = DateTime.Now;
        public decimal precio { set; get; } = 0.0M;
        public double pesoKg { set; get; } = 0.0;
        public int cantidadTotal { set; get; } = 0;
    }
}

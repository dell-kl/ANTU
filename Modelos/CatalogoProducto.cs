using CommunityToolkit.Mvvm.ComponentModel;

namespace Modelos
{
    /*
     Dentro de este fichero definiremos algunas clases mas que estan relacionadas con lo que es 
    nuestro Catalog Producto.
     */
    public partial class CatalogoProducto : ObservableObject
    {
        [ObservableProperty]
        private string identificador = Guid.NewGuid().ToString();

        [ObservableProperty]
        private string nombreProducto = "sin_nombre";

        [ObservableProperty]
        private string rutaImagen = null!;

        [ObservableProperty]
        private DateTime fechaCreacion = DateTime.Now;

        [ObservableProperty]
        private int numeroCategorias = 0;

        [ObservableProperty]
        private int totalSacosCatalogo = 0;

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

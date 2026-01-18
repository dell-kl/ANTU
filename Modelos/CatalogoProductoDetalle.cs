using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Modelos
{
    public partial class CatalogoProductoDetalle : ObservableObject
    {
        [ObservableProperty]
        private int ultimaProduccion = 0; //numero de costales hechos en la ultima produccion.

        [ObservableProperty]
        private decimal ultimaVenta = 0.0m; // valor de la ultima venta realizada.

        [ObservableProperty]
        private int categorias = 0; // numero de categorias que hay en total

        [ObservableProperty]
        private int costalesTotal = 0; // numero de costales en total.  

        [ObservableProperty]
        private ObservableCollection<DataImage> imagenes = new ObservableCollection<DataImage>();

        
    }
}

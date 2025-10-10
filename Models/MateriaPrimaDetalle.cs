using System.Collections.ObjectModel;

namespace ANTU.Models
{
    public class MateriaPrimaDetalle
    {
        public double KgTotal { set; get; } = 0.0d;
        public int TotalCompras { set; get; }
        public decimal UltimaCompra { set; get; } = 0.0m;

        public ObservableCollection<KgSeguimiento> KgSeguimiento { set; get; } = new ObservableCollection<KgSeguimiento>();
    }
}

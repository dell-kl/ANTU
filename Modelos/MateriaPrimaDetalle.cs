using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Modelos
{
    public class MateriaPrimaDetalle : ObservableObject
    {
        private double kgTotal;
        private int totalCompras;
        private decimal ultimaCompra;
        private ObservableCollection<DataImage> _imagenes = new ObservableCollection<DataImage>();

        public double KgTotal { set => SetProperty(ref kgTotal, value); get => kgTotal; }
        public int TotalCompras { set => SetProperty(ref totalCompras, value); get => totalCompras; }
        public decimal UltimaCompra { set => SetProperty(ref ultimaCompra, value); get => ultimaCompra; }
        
        public ObservableCollection<DataImage> imagenes { set => SetProperty(ref _imagenes, value); get => _imagenes; }
    }


}

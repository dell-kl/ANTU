using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ANTU.Models
{
    public class MateriaPrimaDetalle : ObservableObject
    {
        private double kgTotal;
        private int totalCompras;
        private decimal ultimaCompra;

        public double KgTotal { set => SetProperty(ref kgTotal, value); get => kgTotal; }
        public int TotalCompras { set => SetProperty(ref totalCompras, value); get => totalCompras; }
        public decimal UltimaCompra { set => SetProperty(ref ultimaCompra, value); get => ultimaCompra; }
        
        public ObservableCollection<DataImage> imagenes { set; get; } = new ObservableCollection<DataImage>();
    }


}

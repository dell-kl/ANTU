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
        public ObservableCollection<KgSeguimiento> KgSeguimiento { set; get; } = new ObservableCollection<KgSeguimiento>();

        public ObservableCollection<DataImage> imagenes { set; get; } = new ObservableCollection<DataImage>();
    }

    public class DataImage : ObservableObject
    {
        private string identificador;
        private string url;
        private bool estado;

        public string Identificador { set => SetProperty(ref identificador, value); get => identificador; }
        public string Url { set => SetProperty(ref url, value); get => url; }
        public bool Estado { set => SetProperty(ref estado, value); get => estado; }
    }

    public class RequestDataImage
    {
        public string mensaje { set; get; } = null!;
        public ICollection<DataImage> imagenes = new List<DataImage>();
    }
}

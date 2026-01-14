using CommunityToolkit.Mvvm.ComponentModel;

namespace ANTU.Models
{
    public partial class KgSeguimiento : ObservableObject
    {
        [ObservableProperty]
        private string guid = null!;
        [ObservableProperty]
        private int cantidad = 0;
        [ObservableProperty]
        private double kg = 0;
        [ObservableProperty]
        private decimal precio = 0;
        [ObservableProperty]
        private double kgTotal = 0;
        [ObservableProperty]
        private decimal precioTotal = 0;
        [ObservableProperty]
        private DateTime fechaCreacion = DateTime.Now;
        [ObservableProperty]
        private DateTime fechaActualizacion = DateTime.Now;

    }
}

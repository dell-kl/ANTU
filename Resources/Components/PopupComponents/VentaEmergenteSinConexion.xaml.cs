using System.ComponentModel;
using System.Runtime.CompilerServices;
using ANTU.ViewModel.PopupServicesViewModel;

namespace ANTU.Resources.Components.PopupComponents;

public partial class VentaEmergenteSinConexion : INotifyPropertyChanged
{
    private string? descripcion = "No se pudo completar la solicitud por falta de conexión a internet. Por favor, verifica tu conexión e inténtalo de nuevo.";

    public string? Descripcion
    {
        get => descripcion;
        set {
            descripcion = value;
            NotifyPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    private VentanaEmergenteSinConexionViewModel itemViewModel;
    
    public VentaEmergenteSinConexion( VentanaEmergenteSinConexionViewModel itemViewModel )
    {
        InitializeComponent();
        this.itemViewModel = itemViewModel;
        Contenido.Text = Descripcion;
        BindingContext = this.itemViewModel;
    }
    
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
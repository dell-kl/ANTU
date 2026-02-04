using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ANTU.Resources.Components.PopupComponents;

public partial class VentaEmergenteSinConexion : INotifyPropertyChanged
{
    private string? icono = "conexion_perdida.jpg";
    private string? descripcion = "Error conexion...";

    public string Icono
    {
        get => icono;
        set
        {
            icono = value;
            NotifyPropertyChanged();
        }
    }
    public string? Descripcion
    {
        get => descripcion;
        set {
            descripcion = value;
            NotifyPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public VentaEmergenteSinConexion( )
    {
        InitializeComponent();

        BindingContext = this;

    }
    
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ReintentarPeticionButton_OnClicked(object? sender, EventArgs e)
    {
        
    }
}
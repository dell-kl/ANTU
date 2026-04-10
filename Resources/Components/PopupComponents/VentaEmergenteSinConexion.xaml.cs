using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;

namespace ANTU.Resources.Components.PopupComponents;

public partial class VentaEmergenteSinConexion : INotifyPropertyChanged
{
    private string _icono = "conexion_perdida.jpg";
    private string _descripcion = "Error conexion...";
    private AsyncRelayCommand command;

    public AsyncRelayCommand Command
    {
        get => command;
        set
        {
            command = value;
            NotifyPropertyChanged();
        }
    }
    
    public string Icono
    {
        get => _icono;
        set
        {
            _icono = value;
            NotifyPropertyChanged();
        }
    }
    public string? Descripcion
    {
        get => _descripcion;
        set {
            _descripcion = value;
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

}
using CommunityToolkit.Mvvm.ComponentModel;

namespace ANTU.Models;

public partial class Produccion : ObservableObject
{
    [ObservableProperty]
    private string identificador = "";

    [ObservableProperty] 
    private bool estadoFabricado = false;
    
    [ObservableProperty]
    private double kgTotal = 0;

    [ObservableProperty]
    private int estado = 1;

    [ObservableProperty]
    private string nombreProducto = "";

    [ObservableProperty] 
    private string tipoProduccion = "";
    
    [ObservableProperty]
    private DateTime creado = DateTime.Now;
    
    [ObservableProperty]
    private DateTime actualizado = DateTime.Now;
}
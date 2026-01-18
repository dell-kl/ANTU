using CommunityToolkit.Mvvm.ComponentModel;

namespace Modelos;

public partial class ProductosListos : ObservableObject
{
    [ObservableProperty]
    private int numeroCostales;

    [ObservableProperty]
    private string descripcion;
}
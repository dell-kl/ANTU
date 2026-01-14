using CommunityToolkit.Mvvm.ComponentModel;

namespace ANTU.Models;

public partial class ProductosListos : ObservableObject
{
    [ObservableProperty]
    private int numeroCostales;

    [ObservableProperty]
    private string descripcion;
}
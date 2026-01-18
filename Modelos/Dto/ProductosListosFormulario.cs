using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Modelos.Dto;

public class ProductosListosFormulario : ObservableObject
{
    private string datosVenta;
    private int numeroCostales;

    
    [Required(ErrorMessage = "No dejes en blanco el campo!!!")]
    [Display(Name = "Datos Venta", Prompt = "Datos Venta", GroupName = "Ingresar costales a Bodega")]
    public string DatosVenta
    {
        set => SetProperty(ref datosVenta, value);
        get => datosVenta;
    }
    
    [Range(1, int.MaxValue, ErrorMessage = "Valor a 1 o mayor es valido")]
    [Required(ErrorMessage = "No dejes en blanco el campo!!!")]
    [Display(Name = "Numero Costales", Prompt = "Numero Costales", GroupName = "Ingresar costales a Bodega")]
    public int NumeroCostales
    {
        set => SetProperty(ref numeroCostales, value);
        get => numeroCostales;
    }
}
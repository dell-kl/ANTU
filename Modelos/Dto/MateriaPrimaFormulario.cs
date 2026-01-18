using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.DataForm;
using System.ComponentModel.DataAnnotations;

namespace Modelos.Dto
{
    public partial class MateriaPrimaFormulario : ObservableObject
    {
        private string materiaPrima = string.Empty;
        private double kgStandard = 0.0;
        private double precio;
        private int cantidad;

        [DataFormDisplayOptions(ValidMessage = "Nombre de materia prima ingresado correctamente")]
        [Required(ErrorMessage = "Es obligatorio ingresar el nombre de la materia prima")]
        [DataType(DataType.Text)]
        [Display(Name = "Materia Prima", Prompt = "Ingresa el nombre del producto comprado", GroupName = "Datos Producto")]
        public string MateriaPrima { set => SetProperty(ref materiaPrima, value); get => materiaPrima; }

        [DataFormDisplayOptions(ValidMessage = "Peso estándar ingresado correctamente")]
        [Display(Name = "KG Standard", Prompt = "Ingresa el peso estándar en KG", GroupName = "Datos Compra")]
        [Required(ErrorMessage = "Es obligatorio ingresar el peso estándar en KG")]
        [Range(10, double.MaxValue, ErrorMessage = "El peso estándar debe ser mayor o igual a 10KG")]
        public double KgStandard { set => SetProperty(ref kgStandard, value); get => kgStandard; }

        [DataFormDisplayOptions(ValidMessage = "Precio unitario ingresado correctamente")]
        [Display(Name = "Precio Unitario", Prompt = "Ingresa el precio unitario del producto", GroupName = "Datos Compra")]
        [Required(ErrorMessage = "Es obligatorio ingresar el precio unitario del producto")]
        [Range(5, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor o igual a $5")]
        public double Precio { set => SetProperty(ref precio, value); get => precio; }

        [DataFormDisplayOptions(ValidMessage = "Cantidad adquirida ingresada correctamente")]
        [Display(Name = "Cantidad Adquirida", Prompt = "Ingresa la cantidad adquirida", GroupName = "Datos Compra")]
        [Required(ErrorMessage = "Es obligatorio ingresar la cantidad adquirida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor o igual a 1")]
        public int Cantidad { set => SetProperty(ref cantidad, value); get => cantidad; }
    
    }
}

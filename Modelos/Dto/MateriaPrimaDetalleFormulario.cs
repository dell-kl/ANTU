using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.DataForm;
using System.ComponentModel.DataAnnotations;

namespace Modelos.Dto
{
    public class MateriaPrimaDetalleFormulario : ObservableObject
    {
        private int cantidad;
        private double kgStandard;
        private double precioUnitario;

        [DataFormDisplayOptions(ValidMessage = "Dato ingresado correctamente")]
        [Display(Name = "Cantidad", Prompt = "Numero de costales comprados")]
        [Required(ErrorMessage = "Es obligatorio ingresar una cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes ingresar por lo menos que has adquirido un costal o mas.")]
        public int Cantidad { set => SetProperty(ref cantidad, value); get => cantidad; }

        [DataFormDisplayOptions(ValidMessage = "KG permitidos exitosamente")]
        [Display(Name = "KG Estandard", Prompt = "El KG que pesa en un saco")]
        [Required(ErrorMessage = "Es obligatorio ingresar un kilogramo estandar")]
        [Range(5.00, double.MaxValue, ErrorMessage = "No debes dejar en cero los KG porque seran usados para agregar mas a tu stock")]
        public double KGStandard { set => SetProperty(ref kgStandard, value); get => kgStandard; }

        [DataFormDisplayOptions(ValidMessage = "Precio ingresado aceptado")]
        [Display(Name = "Precio Unitario", Prompt = "Precio unitario de venta")]
        [Required(ErrorMessage = "Es obligatorio ingresar el precio unitario")]
        [Range(5.00, double.MaxValue, ErrorMessage = "Ingresa el precio para mantener un historial de todas tus compras realizadas")]
        public double PrecioUnitario { set => SetProperty(ref precioUnitario, value); get => precioUnitario; }
    }
}

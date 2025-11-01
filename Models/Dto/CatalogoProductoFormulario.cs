using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.DataForm;
using System.ComponentModel.DataAnnotations;

namespace ANTU.Models.Dto
{
    public class CatalogoProductoFormulario : ObservableObject
    {
        private string? nombreProducto;

        [DataFormDisplayOptions(ValidMessage = "Nombre ingresado correctamente", RowOrder = 0)]
        [Display(Name = "Nombre Del Producto", Prompt = "Balanceado natural", GroupName = "Datos Producto")]
        [Required(AllowEmptyStrings = false,ErrorMessage = "Es necesario ingresar el nombre del producto")]
        [DataType(DataType.Text)]
        public string? NombreProducto { set => SetProperty(ref nombreProducto, value); get => nombreProducto; }

        public DatosVentas datosVentas { set; get; } = new DatosVentas();
    }

    public class DatosVentas : ObservableObject
    {
        private double precio;
        private double kg;
        private double cantidad;

        [DataFormDisplayOptions(ValidMessage = "Precio ingresado correctamente")]
        [Display(Name = "Precio", Prompt = "$15.50", GroupName = "Datos Venta")]
        [Required(ErrorMessage = "Es obligatorio ingresar un precio de venta mayor a 0")]
        [Range(10, double.MaxValue, ErrorMessage = "Debes ingresar un precio mayor o igual a 10")]
        public double Precio { set => SetProperty(ref precio, value); get => precio; }

        [DataFormDisplayOptions(ValidMessage = "Peso ingresado correctamente")]
        [Display(Name = "KG", Prompt = "KG 30.50", GroupName = "Datos Venta")]
        [Required(ErrorMessage = "Es obligatorio ingresar un peso mayor a 0")]
        [Range(20, double.MaxValue, ErrorMessage = "El peso debe ser mayor 0 o igual a 20KG (Recomendado poner un valor mayor)")]
        public double Kg { set => SetProperty(ref kg, value); get => kg; }

        [DataFormDisplayOptions()]
        [Display(Name = "Cantidad Total Actualmente", GroupName = "Datos Venta", Prompt = "Este campo no es obligatorio, si conoces cuanto tienes actualmente en stock rellenalo o sino dejalo en blanco")]
        public double Cantidad { set => SetProperty(ref cantidad, value); get => cantidad; }
    }
}

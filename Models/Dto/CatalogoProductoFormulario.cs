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

        private DatosVentas datosVentas = new DatosVentas(0.0,0.0,0);

        public DatosVentas DatosVentas { set => SetProperty(ref datosVentas, value); get => datosVentas; } 


        public void limpiarDatos()  
        {
            NombreProducto = string.Empty;
            DatosVentas = new DatosVentas(0.0, 0.0, 0);
        }
    }

    public class DatosVentas : ObservableObject
    {
        private double precio;
        private double kg;
        private int cantidad;

        public DatosVentas(double precio, double kg, int cantidad)
        {
            this.Precio = precio;
            this.Kg = kg;
            this.Cantidad = cantidad;
        }

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
        [Range(0, int.MaxValue, ErrorMessage = "Este campo no puede quedar vacio, si no sabes cuanto tienes en stock no hay problema solo deja en 0 esta campo, caso contrario si lo sabes ingresa tu cantida aqui.")]
        [Required(ErrorMessage = "Este campo no puede quedar vacio, si no sabes cuanto tienes en stock no hay problema solo deja en 0 esta campo, caso contrario si lo sabes ingresa tu cantida aqui.\"")]
        [Display(Name = "Cantidad Total Actualmente", GroupName = "Datos Venta", Prompt = "Si tienes costales en base a la categoria de KG que estas creando de este producto, puedes ingresar el numero aqui, sino dejalo en 0.")]
        public int Cantidad { set => SetProperty(ref cantidad, value); get => cantidad; }
    }
}

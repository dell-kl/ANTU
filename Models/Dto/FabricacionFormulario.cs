using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.DataForm;
using System.ComponentModel.DataAnnotations;

namespace ANTU.Models.Dto
{
    public partial class FabricacionFormulario : ObservableObject
    {
        private string catalogoProducto;
        private string dataVenta;
        private DatosVentaCatalogoProducto datosVentaCatalogoProducto;
        
        [DataFormDisplayOptions(ValidMessage = "Opcion escogida exitosamente")]
        [Required(ErrorMessage = "Es necesario escoger una opcion")]
        [Display(Name = "Catalogo Producto", Prompt = "Escoge el producto que vendes", GroupName = "Catalogo Producto")]
        public string CatalogoProducto  {  set => SetProperty(ref catalogoProducto, value); get => catalogoProducto; } 
        
        [DataFormDisplayOptions(ValidMessage = "Opcion escogida exitosamente")]
        [Required(ErrorMessage = "Es necesario escoger una categoria de venta que tiene el producto")]
        [Display(Name = "Categoria Venta", Prompt = "Escoge precio/kg para vender", GroupName = "Catalogo Producto")]
        public string DataVenta { set => SetProperty(ref dataVenta, value); get => dataVenta; }


        public DatosVentaCatalogoProducto DatosVentaCatalogoProducto
        {
            set => SetProperty(ref datosVentaCatalogoProducto, value);
            get => datosVentaCatalogoProducto;
        }
    }

    public partial class DatosVentaCatalogoProducto : ObservableObject
    {
        private string materiaPrima;
        private double cantidad;
        
        [Display(Name = "Materia Prima", Prompt = "Tu materia prima a usar", GroupName = "Materia Prima a Usar")]
        public string MateriaPrima
        {
            set => SetProperty(ref materiaPrima, value);
            get => materiaPrima;
        }
        [Display(Name = "Cantidad (KG)", Prompt = "cantidad a usar en KG", GroupName = "Materia Prima a Usar")]
        public double Cantidad
        {
            set => SetProperty(ref cantidad, value);
            get => cantidad;
        }
    }
}

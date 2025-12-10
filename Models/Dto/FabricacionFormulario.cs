using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.DataForm;
using System.ComponentModel.DataAnnotations;

namespace ANTU.Models.Dto
{
    public partial class FabricacionFormulario : ObservableObject
    {
        private object catalogoProducto;
        private string dataVenta;
        
        //[Display(Prompt = "Seleccione el catálogo de productos")]
        //public b CatalogoProductos
        //{
        //    get => catalogoProductos;
        //    set => SetProperty(ref catalogoProductos, value);
        //}

        //public object CatalogoProductos { set => SetProperty(ref catalogoProductos, value); get => catalogoProductos; }

        [DataFormDisplayOptions(ValidMessage = "Opcion escogida exitosamente")]
        [Required(ErrorMessage = "Es necesario escoger una opcion")]
        [Display(Name = "Catalogo Producto", Prompt = "Escoge el producto que vendes", GroupName = "CatalogoProducto" )]
        [EnumDataType(typeof(DataFormComboBoxItem))]
        public object CatalogoProducto  {  set => SetProperty(ref catalogoProducto, value); get => catalogoProducto; } 
        
        [DataFormDisplayOptions(ValidMessage = "Opcion escogida exitosamente")]
        [Required(ErrorMessage = "Es necesario escoger una categoria de venta que tiene el producto")]
        [Display(Name = "Categoria Venta", Prompt = "Escoge el Precio vinculada a su KG para vender", GroupName = "CatalogoProducto")]
        // [DataFormValueConverter(typeof(DataFormComboBoxItem))]
        [EnumDataType(typeof(DataFormComboBoxItem))]
        public string DataVenta { set => SetProperty(ref dataVenta, value); get => dataVenta; }
    }   
}

using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ANTU.ViewModel
{
    public partial class CatalogoProductoFormularioViewModel : ParentViewModel
    {
        public CatalogoProductoFormulario catalogoProductoFormulario { set; get; } = new CatalogoProductoFormulario();

        public CatalogoProductoFormularioViewModel(IRestManagement restManagement)
            : base(restManagement)
        {
            
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public override async Task SeleccionarArchivoMostrar()
        {
            await base.SeleccionarArchivoMostrar();
        }

        //este codigo de aqui tiene que ver con la parte de nuestras imagenes, el 
        //boton con la funcionalidad de eliminar la imagen de la lista 
        //este metodo es de prueba solamente, mas adelante hay que reemplazarlo.
        [RelayCommand(AllowConcurrentExecutions = false)]
        public override async Task EliminarArchivoPrueba(string codigo)
        {
            await base.EliminarArchivoPrueba(codigo);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task RegistarCatalogoProducto()
        {
            await _restManagement.CatalogoProduct.Add(
                new Models.RequestDto.CatalogoProductoRequestDto() { 
                    nombreProducto = catalogoProductoFormulario.NombreProducto!,
                    dataCatalogProducts = new List<DataProduct>()
                    {
                        new DataProduct()
                        {
                            precio = (decimal) catalogoProductoFormulario.datosVentas.Precio,
                            pesoKg = catalogoProductoFormulario.datosVentas.Kg,
                            cantidadTotal = 0
                        }
                    }
                }, 
                async () => { }
            );
        }
    }
}

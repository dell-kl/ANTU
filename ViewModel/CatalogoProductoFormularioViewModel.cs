using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ANTU.ViewModel
{
    public partial class CatalogoProductoFormularioViewModel : ParentViewModel
    {
        public CatalogoProductoFormulario catalogoProductoFormulario { set; get; } = new CatalogoProductoFormulario();

        

        public CatalogoProductoFormularioViewModel(IRestManagement restManagement, IPopupService popupService)
            : base(restManagement, popupService)
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
            
            await MostrarSpinner();
            string identificador = Guid.NewGuid().ToString();

            bool resultado = await _restManagement.CatalogoProduct.Add(
                new Models.RequestDto.CatalogoProductoRequestDto()
                {
                    identificador = identificador,
                    nombreProducto = catalogoProductoFormulario.NombreProducto!,
                    dataCatalogProducts = new List<DataProduct>()
                    {
                        new DataProduct()
                        {
                            precio = (decimal) catalogoProductoFormulario.datosVentas.Precio,
                            pesoKg = catalogoProductoFormulario.datosVentas.Kg,
                            cantidadTotal = catalogoProductoFormulario.datosVentas.Cantidad
                        }
                    }
                },
                () => DesmontarSpinner()
            );

            if (resultado && FileManyResults.Any())
            {
                var tareaImagenes = _restManagement.CatalogoProduct.SaveImages(
                    FileManyResults,
                    identificador,
                    activarVentanasAlerta: false
                );
            }

            FileManyResults.Clear();
        }
    }
}
